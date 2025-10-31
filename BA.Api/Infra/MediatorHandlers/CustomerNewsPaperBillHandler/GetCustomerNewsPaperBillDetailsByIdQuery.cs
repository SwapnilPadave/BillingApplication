using BA.Database;
using BA.Dtos.BillDto;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerNewsPaperBillHandler
{
    public class GetCustomerNewsPaperBillDetailsByIdQuery : IRequest<Result>
    {
        public int BillId { get; set; }

        public class GetCustomerNewsPAperBillDetailsByIdHandler : IRequestHandler<GetCustomerNewsPaperBillDetailsByIdQuery, Result>
        {
            private readonly DapperServiceHelper _dapperServiceHelper;
            public GetCustomerNewsPAperBillDetailsByIdHandler(DapperServiceHelper dapperServiceHelper)
            {
                _dapperServiceHelper = dapperServiceHelper;
            }

            public async Task<Result> Handle(GetCustomerNewsPaperBillDetailsByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@BillId", request.BillId);

                    var result = await _dapperServiceHelper.QueryMultipleAsync("Usp_GetCustomerNewsPaperBillDetailsById", param,
                        multi =>
                        {
                            GetCustomerBillDetailsDto? billDetails = multi.ReadFirstOrDefault<GetCustomerBillDetailsDto>() ?? throw new Exception("Bill details not found."); ;
                            var newsPapersDetails = multi.Read<NewsPaperIdAndAmountDetailsDto>().AsEnumerable();

                            billDetails.NewsPapersDetails = newsPapersDetails.ToList();

                            return billDetails;
                        });

                    return Result.Success(result);
                }
                catch (Exception ex)
                {
                    return Result.Failure(new Error(ex.Message));
                }
            }
        }
    }
}
