using Azure.Core;
using BA.Database;
using BA.Database.Infra;
using BA.Dtos.LoginDto;
using BA.Entities.Token;
using BA.Entities.Users;
using BA.Service.Email;
using BA.Utility;
using BA.Utility.Result;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BA.Service.Login
{
    public class LoginService : ILoginService
    {
        private readonly SqlCommands _sqlCommands;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly DapperServiceHelper _dapper;
        public LoginService(SqlCommands sqlCommands
            , IUnitOfWork unitOfWork
            , IEmailService emailService,
DapperServiceHelper dapper)
        {
            _sqlCommands = sqlCommands;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _dapper = dapper;
        }

        public async Task<GetLoginDetails> GetLoginDetails(string userId, string password, CancellationToken cancellationToken)
        {
            var encryptedPassword = Utils.Encrypt(password);
            var data = await _unitOfWork.UserLoginMappingRepository.GetLoginDetailsAsync(userId, encryptedPassword);

            // Deactivate previous active tokens on new login
            try
            {
                var userLogin = await _unitOfWork.TokenRepository.GetAllAsync(x => x.UserId == data.UserId && x.IsActive);
                foreach (var t in userLogin)
                {
                    t.IsActive = false;
                    _unitOfWork.TokenRepository.Update(t);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
            }

            return data;
        }

        public async Task<Result> Logout(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var userLogin = await _unitOfWork.TokenRepository.GetAllAsync(x => x.UserId == userId && x.IsActive);
                foreach (var t in userLogin)
                {
                    t.IsActive = false;
                    _unitOfWork.TokenRepository.Update(t);
                }
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA101"));
            }
        }

        #region Otp generate code
        //public async Task<Result> GenerateAndSendOtp(RegisterUserDto request, CancellationToken cancellationToken)
        //{
        //    var isUserExists = await _unitOfWork.UserRepository.IsUserExistsAsync(request.Email);

        //    if (isUserExists)
        //    {
        //        var random = new Random();
        //        var otp = random.Next(0001, 9999);

        //        string subject = "Your OTP Code";
        //        string body = $"Your OTP code is: {otp}";

        //        var isEmailSent = await _emailService.SendEmailAsync(request.Email, subject, body);
        //        if (isEmailSent)
        //            return Result.Success();
        //        return Result.Failure(new Error("BA506"));
        //    }
        //    else
        //    {
        //        return Result.Failure(new Error("BA504"));
        //   }
        //}
        #endregion  

        public async Task<Result> RegisterUserAsync(RegisterUserDto request, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = await _unitOfWork.UserRepository.IsUserExistsAsync(request.Email, request.MobileNumber);
                if (user == null)
                {
                    return Result.Failure(new Error("BA504"));
                }
                var userLogin = new UserLoginMapping();
                userLogin.UserId = user.Id;
                userLogin.Username = user.Email;
                userLogin.Password = Utils.Encrypt(user.MobileNumber);
                userLogin.IsActive = true;
                userLogin.CreatedBy = 1;
                userLogin.CreatedDate = DateTime.Now;
                userLogin.IsAdmin = false;

                await _unitOfWork.UserLoginMappingRepository.AddAsync(userLogin);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA507"));
            }
        }

        public async Task<Result> GetRefreshToken(string refreshToken, string jwtKey, int expireInMin, string jwtIssuer, string jwtAudience)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RefreshToken", refreshToken);

                var tokenData = await _dapper.QueryFirstOrDefaultAsync<JwtToken>("Usp_GetTokenByRefreshToken", param);

                if (tokenData == null || tokenData.IsActive == false || tokenData.RefreshTokenExpireAt < DateTime.Now)
                {
                    return Result.Failure(new Error("BA505"));
                }

                var paramUser = new DynamicParameters();
                paramUser.Add("@UserId", tokenData.UserId);
                var user = await _dapper.QueryFirstOrDefaultAsync<GetLoginDetails>("USP_GetUserLoginDetails", paramUser);
                if (user == null) return Result.Failure(new Error("BA504"));

                // Create new access token
                var claims = new List<Claim>
                    {
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("IsActive", user.IsActive.ToString().ToLower()),
                        new Claim("Admin", user.Admin.ToString().ToLower()),
                        new Claim("Role", user.Admin ? "Admin" : "User")
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var newAccessExpiry = DateTime.Now.AddMinutes(expireInMin);
                var newJwt = new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: jwtAudience,
                    claims: claims,
                    expires: newAccessExpiry,
                    signingCredentials: creds
                );

                var newAccessTokenString = new JwtSecurityTokenHandler().WriteToken(newJwt);

                // Deactivate previous active access tokens for this user
                var paramDeactivate = new DynamicParameters();
                paramDeactivate.Add("@UserId", user.UserId);
                await _dapper.ExecuteAsync("Usp_DeactivateAccessTokensForUser", paramDeactivate);

                // Insert new access token with same refresh token and expiry
                var paramSave = new DynamicParameters();
                paramSave.Add("@UserId", user.UserId);
                paramSave.Add("@Token", newAccessTokenString);
                paramSave.Add("@ExpireAt", newAccessExpiry);
                paramSave.Add("@RefreshToken", tokenData.RefreshToken);
                paramSave.Add("@RefreshTokenExpireAt", tokenData.RefreshTokenExpireAt); // unchanged
                await _dapper.ExecuteAsync("Usp_InsertTokenDetails", paramSave);

                return Result.Success(new
                {
                    Token = newAccessTokenString,
                    Expiration = newAccessExpiry,
                    RefreshToken = tokenData.RefreshToken,
                    RefreshTokenExpireAt = tokenData.RefreshTokenExpireAt
                });
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ex.Message));
            }
        }
    }
}
