using BA.Dtos.UserDtos;
using BA.Utility.Result;

namespace BA.Service.Users
{
    public interface IUserService
    {
        Task<Result> AddUserAsync(AddUserDto dto, CancellationToken cancellationToken);
        Task<Result> GetUsersAsync(CancellationToken cancellationToken);
        Task<Result> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result> UpdateUserAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken);
        Task<Result> DeleteUserAsync(int id, CancellationToken cancellationToken);
    }
}
