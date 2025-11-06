using BA.Database;
using BA.Service.CurrentUserHelper;
using BA.Utility.Content;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerNewsPaperBillHandler
{
    public class UpdateCustomerNewsPapaerBillStatusQuery : IRequest<Result>
    {
        public int Id { get; set; }
        public bool IsBillPaid { get; set; }

        public class UpdateCustomerNewsPaperBillStatusHandler : IRequestHandler<UpdateCustomerNewsPapaerBillStatusQuery, Result>
        {
            private readonly DapperServiceHelper _dapperServiceHelper;
            private readonly ICurrentUserService _currentUser;
            public UpdateCustomerNewsPaperBillStatusHandler(DapperServiceHelper dapperServiceHelper, ICurrentUserService currentUser)
            {
                _dapperServiceHelper = dapperServiceHelper;
                _currentUser = currentUser;
            }
            public async Task<Result> Handle(UpdateCustomerNewsPapaerBillStatusQuery request, CancellationToken cancellationToken)
            {
                var param = new DynamicParameters();
                param.Add("@UserId", _currentUser.UserId);
                param.Add("@Id", request.Id);
                param.Add("@IsBillPaid", request.IsBillPaid);

                var rowsAffected = await _dapperServiceHelper.ExecuteAsync("Usp_UpdateCustomerNewsPaperBillStatus", param);
                if (rowsAffected > 0)
                {
                    return Result.Success();
                }
                return Result.Failure(new Error("BA1202"));
            }
        }
    }
}
