using BA.Database.Infra;
using BA.Dtos.BillDto;
using BA.Utility.Result;
using Microsoft.Data.SqlClient;

namespace BA.Service.Bill
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlCommand _sqlCommand;
        public BillService(IUnitOfWork unitOfWork
            , SqlCommand sqlCommand)
        {
            _unitOfWork = unitOfWork;
            _sqlCommand = sqlCommand;
        }

        //public async Task<Result> AddCustomerBillDetails(int userId, AddCustomerBillDetailsDto dto)
        //{

        //}
    }
}
