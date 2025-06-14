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
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
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

                await transaction.CommitAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ex.Message));
            }
        }

        public async Task<Result> GetUsersAsync(CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.UserRepository.GetUsersAsync(cancellationToken);

            if (data == null || !data.Any())
            {
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
            }
            return Result.Success(data);
        }

        public async Task<Result> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id);
            if (user == null)
            {
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
            }
            return Result.Success(user);
        }

        public async Task<Result> UpdateUserAsync(int userId, int id, UpdateUserDto dto, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(id);
                if (user == null)
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
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
                await transaction.CommitAsync(cancellationToken);
                return Result.Success(user);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ex.Message));
            }
        }

        public async Task<Result> DeleteUserAsync(int userId, int id, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(id);
                if (user == null)
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
                }

                user.IsActive = false;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = userId;
                _unitOfWork.UserRepository.Update(user);

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ex.Message));
            }
        }
    }
}
