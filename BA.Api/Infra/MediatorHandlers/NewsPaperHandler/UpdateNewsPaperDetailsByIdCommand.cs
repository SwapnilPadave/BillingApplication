using BA.Database;
using BA.Service.CurrentUserHelper;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.NewsPaperHandler
{
    public class UpdateNewsPaperDetailsByIdCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;

        public class UpdateNewsPaperDetailsByIdCommandHandler : IRequestHandler<UpdateNewsPaperDetailsByIdCommand, Result>
        {
            private readonly SqlCommands _sqlCommand;
            private readonly DapperServiceHelper _dapper;
            private readonly ICurrentUserService _currentUser;
            public UpdateNewsPaperDetailsByIdCommandHandler(SqlCommands sqlCommand, DapperServiceHelper dapper, ICurrentUserService currentUser)
            {
                _sqlCommand = sqlCommand;
                _dapper = dapper;
                _currentUser = currentUser;
            }
            public async Task<Result> Handle(UpdateNewsPaperDetailsByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", request.Id);
                    param.Add("@Name", request.Name);
                    param.Add("@Language", request.Language);
                    param.Add("@ModifiedBy", _currentUser.UserId);

                    var rowsAffected = await _dapper.ExecuteAsync("Usp_UpdateNewsPaperDetailsById", param);
                    if (rowsAffected > 0)
                    {
                        return Result.Success();
                    }
                    return Result.Failure(new Error("BA1001"));
                }
                catch (Exception ex)
                {
                    await _sqlCommand.ExceptionLogToDatabase(ex);
                    return Result.Failure(new Error(ex.Message));
                }
            }
        }
    }
}
