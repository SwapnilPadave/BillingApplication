using BA.Api.Infra.Authentication;
using BA.Database;
using BA.Dtos.LoginDto;
using BA.Entities.Token;
using BA.Utility.Result;
using Dapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BA.Api.Infra.MediatorHandlers.AuthHandler
{
    public class RefreshTokenCommand : IRequest<Result>
    {
        public string RefreshToken { get; set; } = string.Empty;

        public class Handler : IRequestHandler<RefreshTokenCommand, Result>
        {
            private readonly DapperServiceHelper _dapper;
            private readonly JwtOptions _jwtOptions;
            public Handler(DapperServiceHelper dapper, IOptions<JwtOptions> jwtOptions)
            {
                _dapper = dapper;
                _jwtOptions = jwtOptions.Value;
            }

            #region update same token entry
            //    public async Task<Result> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            //    {
            //        try
            //        {
            //            Console.WriteLine("🔄 RefreshToken endpoint hit for refreshToken: " + request.RefreshToken);

            //            // Get token record
            //            var param = new DynamicParameters();
            //            param.Add("@RefreshToken", request.RefreshToken);

            //            var tokenData = await _dapper.QueryFirstOrDefaultAsync<JwtToken>("Usp_GetTokenByRefreshToken", param);
            //            if (tokenData == null || !tokenData.IsActive || tokenData.RefreshTokenExpireAt < DateTime.Now)
            //            {
            //                return Result.Failure(new Error("Refresh token expired or invalid."));
            //            }

            //            // Load user details
            //            var paramUser = new DynamicParameters();
            //            paramUser.Add("@UserId", tokenData.UserId);
            //            var user = await _dapper.QueryFirstOrDefaultAsync<GetLoginDetails>("USP_GetUserLoginDetails", paramUser);
            //            if (user == null) return Result.Failure(new Error("User not found"));

            //            // Create new access token
            //            var claims = new List<Claim>
            //            {
            //                new Claim("UserId", user.UserId.ToString()),
            //                new Claim("UserName", user.UserName),
            //                new Claim("IsActive", user.IsActive.ToString().ToLower()),
            //                new Claim("Admin", user.Admin.ToString().ToLower()),
            //                new Claim("Role", user.Admin ? "Admin" : "User")
            //            };

            //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //            var newAccessExpiry = DateTime.Now.AddMinutes(_jwtOptions.ExpiryMinutes);
            //            var newJwt = new JwtSecurityToken(
            //                issuer: _jwtOptions.Issuer,
            //                audience: _jwtOptions.Audience,
            //                claims: claims,
            //                expires: newAccessExpiry,
            //                signingCredentials: creds
            //            );

            //            var newAccessTokenString = new JwtSecurityTokenHandler().WriteToken(newJwt);

            //            // Update only access token, keep refresh token and expiry unchanged
            //            var paramUpdateAccess = new DynamicParameters();
            //            paramUpdateAccess.Add("@TokenId", tokenData.Id); // or use UserId+IsActive=1
            //            paramUpdateAccess.Add("@NewToken", newAccessTokenString);
            //            paramUpdateAccess.Add("@NewExpireAt", newAccessExpiry);

            //            await _dapper.ExecuteAsync("Usp_UpdateAccessTokenOnly", paramUpdateAccess);

            //            return Result.Success(new
            //            {
            //                Token = newAccessTokenString,
            //                Expiration = newAccessExpiry,
            //                RefreshToken = tokenData.RefreshToken,
            //                RefreshTokenExpireAt = tokenData.RefreshTokenExpireAt
            //            });
            //        }
            //        catch (Exception ex)
            //        {
            //            return Result.Failure(new Error("Error occurred while refreshing token."));
            //        }
            //    }
            //} 
            #endregion

            public async Task<Result> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    Console.WriteLine("🔄 RefreshToken endpoint hit for refreshToken: " + request.RefreshToken);
                    var param = new DynamicParameters();
                    param.Add("@RefreshToken", request.RefreshToken);

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

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var newAccessExpiry = DateTime.Now.AddMinutes(_jwtOptions.ExpiryMinutes);
                    var newJwt = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        audience: _jwtOptions.Audience,
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
}
