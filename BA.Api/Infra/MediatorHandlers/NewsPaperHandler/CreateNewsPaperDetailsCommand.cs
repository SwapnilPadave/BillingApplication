using BA.Database;
using BA.Service.CurrentUserHelper;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.NewsPaperHandler
{
    public class CreateNewsPaperDetailsCommand : IRequest<Result>
    {
        public string Name { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;

        public class CreateNewsPaperDetailsCommandHandler : IRequestHandler<CreateNewsPaperDetailsCommand, Result>
        {
            private readonly ICurrentUserService _currentUser;
            private readonly DapperServiceHelper _dapper;
            private readonly SqlCommands _sqlCommands;
            public CreateNewsPaperDetailsCommandHandler(ICurrentUserService currentUser, DapperServiceHelper dapper, SqlCommands sqlCommands)
            {
                _currentUser = currentUser;
                _dapper = dapper;
                _sqlCommands = sqlCommands;
            }
            public async Task<Result> Handle(CreateNewsPaperDetailsCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@Name", request.Name);
                    param.Add("@Language", request.Language);
                    param.Add("@CreatedBy", _currentUser.UserId);

                    var rowsAffected = await _dapper.ExecuteAsync("Usp_InsertNewsPaperDetails", param);
                    if (rowsAffected > 0)
                    {
                        return Result.Success("Success.");
                    }
                    return Result.Failure(new Error("Failed to create newspaper details."));
                }
                catch (Exception ex)
                {
                    await _sqlCommands.ExceptionLogToDatabase(ex);
                    return Result.Failure(new Error(ex.Message));
                }
            }
        }
    }
}
