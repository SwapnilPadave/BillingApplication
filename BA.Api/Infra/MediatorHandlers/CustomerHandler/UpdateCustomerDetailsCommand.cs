using BA.Database;
using BA.Service.CurrentUserHelper;
using BA.Utility.Content;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerHandler
{
    public class UpdateCustomerDetailsCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string BuildingName { get; set; } = string.Empty;
        public string RoomNo { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;

        public class UpdateCustomerDetailsCommandHandler : IRequestHandler<UpdateCustomerDetailsCommand, Result>
        {
            private readonly ICurrentUserService _currentUser;
            private readonly DapperServiceHelper _dapper;
            private readonly SqlCommands _sqlCommands;
            public UpdateCustomerDetailsCommandHandler(ICurrentUserService currentUser, DapperServiceHelper dapper, SqlCommands sqlCommands)
            {
                _currentUser = currentUser;
                _dapper = dapper;
                _sqlCommands = sqlCommands;
            }
            public async Task<Result> Handle(UpdateCustomerDetailsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", request.Id);
                    param.Add("@BuildingName", request.BuildingName);
                    param.Add("@RoomNo", request.RoomNo);
                    param.Add("@AreaName", request.AreaName);
                    param.Add("@ModifiedBy", _currentUser.UserId);

                    var rowsAffected = await _dapper.ExecuteAsync("Usp_UpdateCustomerDetailsById", param);
                    if (rowsAffected > 0)
                    {
                        return Result.Success();
                    }
                    return Result.Failure(new Error("BA1105"));
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
