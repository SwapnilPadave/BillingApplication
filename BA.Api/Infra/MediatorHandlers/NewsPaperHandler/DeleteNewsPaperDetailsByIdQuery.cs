using BA.Database;
using BA.Service.CurrentUserHelper;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.NewsPaperHandler
{
    public class DeleteNewsPaperDetailsByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }

        public class DeleteNewsPaperDetailsByUserIdQueryHandler : IRequestHandler<DeleteNewsPaperDetailsByIdQuery, Result>
        {
            private readonly DapperServiceHelper _dapper;
            private readonly ICurrentUserService _currentUser;
            public DeleteNewsPaperDetailsByUserIdQueryHandler(DapperServiceHelper dapper, ICurrentUserService currentUser)
            {
                _dapper = dapper;
                _currentUser = currentUser;
            }
            public async Task<Result> Handle(DeleteNewsPaperDetailsByIdQuery request, CancellationToken cancellationToken)
            {
                var param = new DynamicParameters();
                param.Add("@Id", request.Id);
                param.Add("@ModifiedBy", _currentUser.UserId);
                param.Add("@IsActive", 1);

                var rowsAffected = await _dapper.ExecuteAsync("Usp_UpdateNewsPaperDetailsStatusById", param);
                if (rowsAffected > 0)
                {
                    return Result.Success("Record deleted successfully.");
                }
                return Result.Failure(new Error("Failed to delete this record."));
            }
        }
    }
}
