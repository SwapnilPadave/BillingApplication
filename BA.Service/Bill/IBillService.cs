using BA.Dtos.BillDto;
using BA.Utility.Result;

namespace BA.Service.Bill
{
    public interface IBillService
    {
        //decimal CalculateTotalAmount(CalculateTotalAmountDto calculateTotalAmountDto);
        Task<Result> AddCustomerBillDetails(int userId, AddCustomerBillDetailsDto dto);
        Task<Result> GetAllCustomerBills();
        Task<Result> GetCustomerBillById(int id);
        Task<Result> UpdateCustomerBillDetails(int userId, int id, UpdateCustomerBillDetailsDto dto);
        Task<Result> DeleteCustomerBill(int userId, int id);
        Task<Result> UpdateBillStatusAsync(int userId, int id, bool isBillPaid);
        Task<Result> GenerateBill(int userId, int id);

        Dictionary<string, int> GetSatAndSunCount(DateTime fromDate, DateTime toDate, int specialDays = 0);
    }
}
