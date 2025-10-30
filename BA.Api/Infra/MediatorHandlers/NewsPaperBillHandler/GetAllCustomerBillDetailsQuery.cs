using BA.Database;
using BA.Entities.Bill;
using BA.Utility.Result;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.NewsPaperBillHandler
{
    public class GetAllCustomerBillDetailsQuery : IRequest<Result>
    {
        public class GetAllCustomerBillDetailsHandler : IRequestHandler<GetAllCustomerBillDetailsQuery, Result>
        {
            private readonly DapperServiceHelper _dapperServiceHelper;
            public GetAllCustomerBillDetailsHandler(DapperServiceHelper dapperServiceHelper)
            {
                _dapperServiceHelper = dapperServiceHelper;
            }

            public async Task<Result> Handle(GetAllCustomerBillDetailsQuery request, CancellationToken cancellationToken)
            {
                var result = await _dapperServiceHelper.QueryListAsync<CustomerBillDetails>("Usp_GetAllCustomerBillDetails");
                return Result.Success(result);
            }
        }
    }
}
