using BA.Database.Infra;
using BA.Dtos.UserDtos;
using BA.Entities.Users;

namespace BA.Database.Repos.UserRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<GetUserDto>> GetUsersAsync(CancellationToken cancellationToken);
        Task<User?> IsUserExistsAsync(string email, string mobileNumber);
    }
}
