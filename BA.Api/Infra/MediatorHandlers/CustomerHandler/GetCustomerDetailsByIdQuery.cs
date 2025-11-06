using BA.Database;
using BA.Entities.Customer;
using BA.Service.CurrentUserHelper;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerHandler
{
    public class GetCustomerDetailsByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
        public class GetCustomerByIdHandler : IRequestHandler<GetCustomerDetailsByIdQuery, Result>
        {
            private readonly DapperServiceHelper _dapper;
            public GetCustomerByIdHandler(DapperServiceHelper dapper)
            {
                _dapper = dapper;
            }
            public async Task<Result> Handle(GetCustomerDetailsByIdQuery request, CancellationToken cancellationToken)
            {
                var param = new DynamicParameters();
                param.Add("@Id", request.Id);

                var customer = await _dapper.QueryFirstOrDefaultAsync<CustomerDetails>("Usp_GetCustomerDetailsById", param);
                if (customer == null)
                {
                    return Result.Failure(new Error("BA502"));
                }
                return Result.Success(customer);
            }
        }
    }
}
