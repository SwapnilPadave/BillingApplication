using BA.Dtos.LoginDto;
using BA.Utility.Result;

namespace BA.Service.Login
{
    public interface ILoginService
    {
        Task<GetLoginDetails> GetLoginDetails(string userId, string password, CancellationToken cancellationToken);
        Task<Result> RegisterUserAsync(RegisterUserDto request, CancellationToken cancellationToken);
        //Task<Result> GenerateAndSendOtp(RegisterUserDto request, CancellationToken cancellationToken);
    }
}
