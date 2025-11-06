using BA.Database;
using BA.Service.CurrentUserHelper;
using BA.Utility.Content;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerHandler
{
    public class UpdateCustomerStatusQuery : IRequest<Result>
    {
        public int Id { get; set; }
        public class UpdateCustomerStatusQueryHandler : IRequestHandler<UpdateCustomerStatusQuery, Result>
        {
            private readonly ICurrentUserService _currentUser;
            private readonly DapperServiceHelper _dapper;
            private readonly SqlCommands _sqlCommands;
            public UpdateCustomerStatusQueryHandler(ICurrentUserService currentUser, DapperServiceHelper dapper, SqlCommands sqlCommands)
            {
                _currentUser = currentUser;
                _dapper = dapper;
                _sqlCommands = sqlCommands;
            }
            public async Task<Result> Handle(UpdateCustomerStatusQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", request.Id);
                    param.Add("@ModifiedBy", _currentUser.UserId);

                    var rowsAffected = await _dapper.ExecuteAsync("Usp_UpdateCustomerStatusById", param);
                    if (rowsAffected > 0)
                    {
                        return Result.Success();
                    }
                    return Result.Failure(new Error("BA1106"));
                }
                catch (Exception ex)
                {
                    await _sqlCommands.ExceptionLogToDatabase(ex);
                    return Result.Failure(new Error("BA501"));
                }
            }
        }
    }
}
