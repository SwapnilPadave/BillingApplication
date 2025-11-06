using BA.Database;
using BA.Service.CurrentUserHelper;
using BA.Utility.Content;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerHandler
{
    public class AddCustomerDetailsCommand : IRequest<Result>
    {
        public string BuildingName { get; set; } = string.Empty;
        public string RoomNo { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;

        public class AddCustomerDetailsCommandHandler : IRequestHandler<AddCustomerDetailsCommand, Result>
        {
            private readonly ICurrentUserService _currentUser;
            private readonly DapperServiceHelper _dapper;
            private readonly SqlCommands _sqlCommands;
            public AddCustomerDetailsCommandHandler(ICurrentUserService currentUser, DapperServiceHelper dapper, SqlCommands sqlCommands)
            {
                _currentUser = currentUser;
                _dapper = dapper;
                _sqlCommands = sqlCommands;
            }

            public async Task<Result> Handle(AddCustomerDetailsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@BuildingName", request.BuildingName);
                    param.Add("@RoomNo", request.RoomNo);
                    param.Add("@AreaName", request.AreaName);
                    param.Add("@CreatedBy", _currentUser.UserId);

                    var rowsAffected = await _dapper.ExecuteAsync("Usp_InsertCustomerDetails", param);
                    if (rowsAffected > 0)
                    {
                        return Result.Success();
                    }
                    return Result.Failure(new Error("BA1104"));
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
