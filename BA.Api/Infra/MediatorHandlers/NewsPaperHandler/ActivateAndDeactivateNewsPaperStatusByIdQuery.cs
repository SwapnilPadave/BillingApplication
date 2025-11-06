using BA.Database;
using BA.Service.CurrentUserHelper;
using BA.Utility.Content;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.NewsPaperHandler
{
    public class ActivateAndDeactivateNewsPaperStatusByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public class ActivateAndDeactivateNewsPaperStatusByIdQueryHandler : IRequestHandler<ActivateAndDeactivateNewsPaperStatusByIdQuery, Result>
        {
            private readonly DapperServiceHelper _dapper;
            private readonly ICurrentUserService _currentUser;

            public ActivateAndDeactivateNewsPaperStatusByIdQueryHandler(DapperServiceHelper dapper, ICurrentUserService currentUser)
            {
                _dapper = dapper;
                _currentUser = currentUser;
            }
            public async Task<Result> Handle(ActivateAndDeactivateNewsPaperStatusByIdQuery request, CancellationToken cancellationToken)
            {
                var param = new DynamicParameters();
                param.Add("@Id", request.Id);
                param.Add("@ModifiedBy", _currentUser.UserId);
                param.Add("@IsActive", request.IsActive ? 1 : 0);

                var rowsAffected = await _dapper.ExecuteAsync("Usp_UpdateNewsPaperDetailsStatusById", param);
                if (rowsAffected > 0)
                {
                    var status = request.IsActive ? "activated" : "deactivated";

                    var replace = new Dictionary<string, string> { { "status", status } };
                    return Result.Success(ContentLoader.ReturnLanguageMessage("BA707", replace));
                }
                return Result.Failure(new Error("BA708"));
            }
        }
    }
}
