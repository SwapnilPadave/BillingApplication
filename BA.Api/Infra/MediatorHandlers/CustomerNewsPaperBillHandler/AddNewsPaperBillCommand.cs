using BA.Database;
using BA.Dtos.BillDto;
using BA.Service.CurrentUserHelper;
using BA.Utility.Content;
using BA.Utility.Result;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BA.Api.Infra.MediatorHandlers.CustomerNewsPaperBillHandler
{
    public class AddNewsPaperBillCommand : IRequest<Result>
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public DateTime? FromDate { get; set; } = DateTime.Now;
        public DateTime? ToDate { get; set; } = DateTime.Now;
        public int TotalDays { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsBillPaid { get; set; }
        public List<NewsPaperIdAndAmount> NewsPaperIdAndAmount { get; set; } = new List<NewsPaperIdAndAmount>();

        protected class AddNewsPaperBillCommandHandler : IRequestHandler<AddNewsPaperBillCommand, Result>
        {
            private readonly SqlCommands _sqlCommand;
            private readonly SqlServiceHelper _sqlServiceHelper;
            private readonly ICurrentUserService _currentUser;
            public AddNewsPaperBillCommandHandler(SqlCommands sqlCommand, SqlServiceHelper sqlServiceHelper, ICurrentUserService currentUser)
            {
                _sqlCommand = sqlCommand;
                _sqlServiceHelper = sqlServiceHelper;
                _currentUser = currentUser;
            }
            public async Task<Result> Handle(AddNewsPaperBillCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var dt = new DataTable();
                    dt.Columns.Add("NewsPaperId", typeof(int));
                    dt.Columns.Add("NormalDays", typeof(int));
                    dt.Columns.Add("SundayDays", typeof(int));
                    dt.Columns.Add("SaturdayDays", typeof(int));
                    dt.Columns.Add("SpecialDays", typeof(int));
                    dt.Columns.Add("NormalDayAmount", typeof(decimal));
                    dt.Columns.Add("SundayAmount", typeof(decimal));
                    dt.Columns.Add("SaturdayAmount", typeof(decimal));
                    dt.Columns.Add("SpecialDayAmount", typeof(decimal));

                    foreach (var item in request.NewsPaperIdAndAmount)
                    {
                        dt.Rows.Add(item.Id, item.NormalDays, item.SundayDays, item.SaturdayDays,
                                    item.SpecialDays, item.NormalDayAmount, item.SundayAmount,
                                    item.SaturdayAmount, item.SpecialDayAmount);
                    }

                    var param = new[]
                    {
                        new SqlParameter("@CustomerName", request.CustomerName),
                        new SqlParameter("@CustomerAddress", request.CustomerAddress),
                        new SqlParameter("@FromDate", request.FromDate),
                        new SqlParameter("@ToDate", request.ToDate),
                        new SqlParameter("@ServiceCharge", request.ServiceCharge),
                        new SqlParameter("@TotalAmount", request.TotalAmount),
                        new SqlParameter("@CreatedBy", _currentUser.UserId),
                        new SqlParameter("@CustomerId", request.CustomerId),
                        new SqlParameter("@NewsPaperList", dt)
                        {
                            SqlDbType=SqlDbType.Structured,
                            TypeName="dbo.NewsPaperEntryType"
                        }
                    };

                    await _sqlServiceHelper.ExecuteNonQueryStoredProcedureAsync("Usp_InsertCustomerBillDetails", param);

                    return await Task.FromResult(Result.Success());
                }
                catch (Exception ex)
                {
                    await _sqlCommand.ExceptionLogToDatabase(ex);
                    return Result.Failure(new Error("BA501"));
                }
            }
        }
    }
}
