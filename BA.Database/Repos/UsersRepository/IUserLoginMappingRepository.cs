using BA.Database.Infra;
using BA.Dtos.LoginDto;
using BA.Entities.Users;

namespace BA.Database.Repos.UsersRepository
{
    public interface IUserLoginMappingRepository : IRepository<UserLoginMapping>
    {
        Task<GetLoginDetails> GetLoginDetailsAsync(string userId, string password);
    }
}
