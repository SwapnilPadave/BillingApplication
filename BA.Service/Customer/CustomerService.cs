using BA.Database;
using BA.Database.Infra;
using BA.Dtos.CustomerDto;
using BA.Entities.Customer;
using BA.Utility.Content;
using BA.Utility.Result;

namespace BA.Service.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlCommands _sqlCommands;
        public CustomerService(IUnitOfWork unitOfWork
            , SqlCommands sqlCommands)
        {
            _unitOfWork = unitOfWork;
            _sqlCommands = sqlCommands;
        }

        public async Task<Result> AddCustomerAsync(int userId, AddCustomerDto customerDto)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var customerDetails = new CustomerDetails
                {
                    BuildingName = customerDto.BuildingName,
                    RoomNo = customerDto.RoomNo,
                    AreaName = customerDto.AreaName,
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };
                await _unitOfWork.CustomerDetailsRepository.AddAsync(customerDetails);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return Result.Success(ContentLoader.ReturnLanguageData("BA1100"));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA101")));
            }
        }

        public async Task<Result> UpdateCustomerAsync(int userId, int id, UpdateCustomerDto customerDto)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var customerDetails = await _unitOfWork.CustomerDetailsRepository.GetAsync(id);
                if (customerDetails == null)
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
                }
                customerDetails.BuildingName = customerDto.BuildingName;
                customerDetails.RoomNo = customerDto.RoomNo;
                customerDetails.AreaName = customerDto.AreaName;
                customerDetails.ModifiedBy = userId;
                customerDetails.ModifiedDate = DateTime.UtcNow;
                _unitOfWork.CustomerDetailsRepository.Update(customerDetails);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return Result.Success(ContentLoader.ReturnLanguageData("BA1101"));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA101")));
            }
        }

        public async Task<Result> DeleteCustomerAsync(int userId, int id)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var customerDetails = await _unitOfWork.CustomerDetailsRepository.GetAsync(id);
                if (customerDetails == null)
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
                }
                customerDetails.IsActive = false;
                customerDetails.ModifiedBy = userId;
                customerDetails.ModifiedDate = DateTime.UtcNow;
                _unitOfWork.CustomerDetailsRepository.Update(customerDetails);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return Result.Success(ContentLoader.ReturnLanguageData("BA1102"));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA101")));
            }
        }

        public async Task<Result> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customerDetails = await _unitOfWork.CustomerDetailsRepository.GetAsync(id);
                if (customerDetails == null)
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
                }
                return Result.Success(customerDetails);
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA101")));
            }
        }

        public async Task<Result> GetAllCustomersAsync()
        {
            try
            {
                var customerDetailsList = await _unitOfWork.CustomerDetailsRepository.GetAllAsync();
                if (customerDetailsList == null || !customerDetailsList.Any())
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
                }
                return Result.Success(customerDetailsList);
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA101")));
            }
        }
    }
}