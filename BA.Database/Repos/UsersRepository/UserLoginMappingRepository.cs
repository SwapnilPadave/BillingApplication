using BA.Database.Infra;
using BA.Entities.Users;

namespace BA.Database.Repos.UsersRepository
{
    public class UserLoginMappingRepository : Repository<UserLoginMapping>, IUserLoginMappingRepository
    {
        private readonly BAContext _context;
        public UserLoginMappingRepository(BAContext context) : base(context)
        {
            _context = context;
        }
    }
}
