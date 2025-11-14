using BA.Database;
using BA.Database.Infra;
using BA.Dtos.UserDtos;
using BA.Entities.Users;
using BA.Utility.Content;
using BA.Utility.Result;

namespace BA.Service.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlCommands _sqlCommands;
        public UserService(IUnitOfWork unitOfWork, SqlCommands sqlCommands)
        {
            _unitOfWork = unitOfWork;
            _sqlCommands = sqlCommands;
        }

        public async Task<Result> AddUserAsync(int userId, AddUserDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var user = new User();

                user.Name = dto.Name;
                user.MobileNumber = dto.MobileNumber;
                user.Email = dto.Email;
                user.Address = dto.Address;
                user.DateOfBirth = dto.DateOfBirth;
                user.Age = dto.Age;
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = userId;
                user.IsActive = true;
                var result = await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> GetUsersAsync(CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.UserRepository.GetUsersAsync(cancellationToken);

            if (data == null || !data.Any())
            {
                return Result.Failure(new Error("BA502"));
            }
            return Result.Success(data);
        }

        public async Task<Result> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserDetailsById(id);
            if (user == null)
            {
                return Result.Failure(new Error("BA502"));
            }
            return Result.Success(user);
        }

        public async Task<Result> UpdateUserAsync(int userId, int id, UpdateUserDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(id);
                if (user == null)
                {
                    return Result.Failure(new Error("BA502"));
                }
                user.Name = dto.Name;
                user.MobileNumber = dto.MobileNumber;
                user.Email = dto.Email;
                user.Address = dto.Address;
                user.DateOfBirth = dto.DateOfBirth;
                user.Age = dto.Age;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = userId;
                _unitOfWork.UserRepository.Update(user);

                await _unitOfWork.SaveChangesAsync();
                return Result.Success(user);
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> DeleteUserAsync(int userId, int id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(id);
                if (user == null)
                {
                    return Result.Failure(new Error("BA502"));
                }

                user.IsActive = false;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = userId;
                _unitOfWork.UserRepository.Update(user);

                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> ActivateOrDeactivateAsync(int userId, int id, bool isActive, CancellationToken cancellationToken)
        {
            try
            {
                var userDetails = await _unitOfWork.UserRepository.GetAsync(id);
                if (userDetails == null)
                {
                    return Result.Failure(new Error("BA502"));
                }
                userDetails.IsActive = isActive;
                userDetails.ModifiedBy = userId;
                userDetails.ModifiedDate = DateTime.Now;
                var result = _unitOfWork.UserRepository.Update(userDetails);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var status = isActive ? "activated" : "deactivated";

                var replace = new Dictionary<string, string> { { "status", status } };
                return Result.Success(ContentLoader.ReturnLanguageMessage("BA509", replace));
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }
    }
}
