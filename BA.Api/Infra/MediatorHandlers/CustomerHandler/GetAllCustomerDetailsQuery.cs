using BA.Database;
using BA.Entities.Customer;
using BA.Utility.Result;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerHandler
{
    public class GetAllCustomerDetailsQuery : IRequest<Result>
    {
        public class GetAllCustomerDetailsQueryHandler : IRequestHandler<GetAllCustomerDetailsQuery, Result>
        {
            private readonly DapperServiceHelper _dapper;
            public GetAllCustomerDetailsQueryHandler(DapperServiceHelper dapper)
            {
                _dapper = dapper;
            }
            public async Task<Result> Handle(GetAllCustomerDetailsQuery request, CancellationToken cancellationToken)
            {
                var customers = await _dapper.QueryListAsync<CustomerDetails>("Usp_GetAllCustomerDetails");
                if (customers.Count > 0)
                {
                    return Result.Success(customers);
                }
                return Result.Failure(new Error("BA502"));
            }
        }
    }
}
