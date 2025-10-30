using BA.Database;
using BA.Service.CurrentUserHelper;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.NewsPaperBillHandler
{
    public class DeleteCustomerNewsPaperBillQuery : IRequest<Result>
    {
        public int Id { get; set; }

        public class DeleteCustomerNewsPaperBillHandler : IRequestHandler<DeleteCustomerNewsPaperBillQuery, Result>
        {
            private readonly DapperServiceHelper _dapperServiceHelper;
            private readonly ICurrentUserService _currentUser;
            public DeleteCustomerNewsPaperBillHandler(DapperServiceHelper dapperServiceHelper, ICurrentUserService currentUser)
            {
                _dapperServiceHelper = dapperServiceHelper;
                _currentUser = currentUser;
            }

            public async Task<Result> Handle(DeleteCustomerNewsPaperBillQuery request, CancellationToken cancellationToken)
            {
                var param = new DynamicParameters();
                param.Add("@Id", request.Id);
                param.Add("@UserId", _currentUser.UserId);

                var rowsAffected = await _dapperServiceHelper.ExecuteAsync("Usp_DeleteCustomerNewsPaperBill", param);
                if (rowsAffected > 0)
                {
                    return Result.Success("Bill deleted successfully");
                }
                return Result.Failure(new Error("Failed to delete bill"));
            }
        }
    }
}
