using BA.Database.Infra;
using BA.Dtos.UserDtos;
using BA.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BA.Database.Repos.UserRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly BAContext _context;
        public UserRepository(BAContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<GetUserDto>> GetUsersAsync(CancellationToken cancellationToken)
        {
            var users = await (from u in _context.Users
                               select new GetUserDto
                               {
                                   Id = u.Id,
                                   Name = u.Name,
                                   MobileNumber = u.MobileNumber,
                                   Email = u.Email,
                                   Address = u.Address,
                                   DateOfBirth = u.DateOfBirth,
                                   Age = u.Age,
                                   IsActive = u.IsActive
                               }).ToListAsync();

            return users;
        }

        public async Task<GetUserDto?> GetUserDetailsById(int id)
        {
            var data = await (from u in _context.Users
                              where u.Id == id
                              select new GetUserDto
                              {
                                  Id = u.Id,
                                  Name = u.Name,
                                  MobileNumber = u.MobileNumber,
                                  Email = u.Email,
                                  Address = u.Address,
                                  DateOfBirth = u.DateOfBirth,
                                  Age = u.Age,
                                  IsActive = u.IsActive
                              }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<User?> IsUserExistsAsync(string email, string mobileNumber)
        {
            var data = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.MobileNumber == mobileNumber);

            return data;

        }
    }
}
