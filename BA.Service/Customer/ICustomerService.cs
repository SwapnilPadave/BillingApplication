using BA.Dtos.CustomerDto;
using BA.Entities.Customer;
using BA.Utility.Result;

namespace BA.Service.Customer
{
    public interface ICustomerService
    {
        Task<Result> AddCustomerAsync(int userId, AddCustomerDto customerDto);
        Task<Result> UpdateCustomerAsync(int userId, int id, UpdateCustomerDto customerDto);
        Task<Result> DeleteCustomerAsync(int userId, int id);
        Task<Result> GetCustomerByIdAsync(int id);
        Task<Result> GetAllCustomersAsync();
    }
}
