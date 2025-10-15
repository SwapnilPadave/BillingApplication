using BA.Database.Infra;
using BA.Dtos.LoginDto;
using BA.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BA.Database.Repos.UsersRepository
{
    public class UserLoginMappingRepository : Repository<UserLoginMapping>, IUserLoginMappingRepository
    {
        private readonly BAContext _context;
        public UserLoginMappingRepository(BAContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GetLoginDetails> GetLoginDetailsAsync(string userId, string password)
        {
            var data = await (from l in _context.UserLoginMappings
                              join u in _context.Users on l.UserId equals u.Id
                              where l.Username == userId && l.Password == password
                              select new GetLoginDetails
                              {
                                  UserId = u.Id,
                                  UserName = u.Name,
                                  EmailAddress = u.Email,
                                  MobileNumber = u.MobileNumber,
                                  IsActive = u.IsActive,
                                  Admin = l.IsAdmin
                              }).FirstOrDefaultAsync();

            return data ?? new GetLoginDetails();
        }
    }
}
