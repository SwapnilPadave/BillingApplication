using Azure.Core;
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
            try
            {
                var customerDetails = new CustomerDetails
                {
                    BuildingName = customerDto.BuildingName,
                    RoomNo = customerDto.RoomNo,
                    AreaName = customerDto.AreaName,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                await _unitOfWork.CustomerDetailsRepository.AddAsync(customerDetails);
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> UpdateCustomerAsync(int userId, int id, UpdateCustomerDto customerDto)
        {
            try
            {
                var customerDetails = await _unitOfWork.CustomerDetailsRepository.GetAsync(customerDto.Id);
                if (customerDetails == null)
                {
                    return Result.Failure(new Error("BA502"));
                }
                customerDetails.BuildingName = customerDto.BuildingName;
                customerDetails.RoomNo = customerDto.RoomNo;
                customerDetails.AreaName = customerDto.AreaName;
                customerDetails.IsActive = true;
                customerDetails.ModifiedBy = userId;
                customerDetails.ModifiedDate = DateTime.Now;
                _unitOfWork.CustomerDetailsRepository.Update(customerDetails);
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> DeleteCustomerAsync(int userId, int id, bool isActive)
        {
            try
            {
                var customerDetails = await _unitOfWork.CustomerDetailsRepository.GetAsync(id);
                if (customerDetails == null)
                {
                    return Result.Failure(new Error("BA502"));
                }
                customerDetails.IsActive = isActive;
                customerDetails.ModifiedBy = userId;
                customerDetails.ModifiedDate = DateTime.Now;
                _unitOfWork.CustomerDetailsRepository.Update(customerDetails);
                await _unitOfWork.SaveChangesAsync();

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

        public async Task<Result> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customerDetails = await _unitOfWork.CustomerDetailsRepository.GetAsync(id);
                if (customerDetails == null)
                {
                    return Result.Failure(new Error("BA502"));
                }
                return Result.Success(customerDetails);
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> GetAllCustomersAsync()
        {
            try
            {
                var customerDetailsList = await _unitOfWork.CustomerDetailsRepository.GetAllAsync();
                if (customerDetailsList == null || !customerDetailsList.Any())
                {
                    return Result.Failure(new Error("BA502"));
                }
                return Result.Success(customerDetailsList);
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<CustomerDetails> GetCustomerByIdForConsumApiAsync(int id)
        {
            return await _unitOfWork.CustomerDetailsRepository.GetAsync(id);
        }
    }
}