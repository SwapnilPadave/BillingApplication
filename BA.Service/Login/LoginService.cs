using BA.Database;
using BA.Database.Infra;
using BA.Dtos.LoginDto;
using BA.Entities.Users;
using BA.Service.Email;
using BA.Utility;
using BA.Utility.Result;

namespace BA.Service.Login
{
    public class LoginService : ILoginService
    {
        private readonly SqlCommands _sqlCommands;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        public LoginService(SqlCommands sqlCommands
            , IUnitOfWork unitOfWork
            , IEmailService emailService)
        {
            _sqlCommands = sqlCommands;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
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
    }
}
