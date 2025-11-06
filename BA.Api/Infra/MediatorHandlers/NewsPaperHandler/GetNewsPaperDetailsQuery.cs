using BA.Database;
using BA.Dtos.NewsPaperDto;
using BA.Service.CurrentUserHelper;
using BA.Utility.Content;
using BA.Utility.Result;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.NewsPaperHandler
{
    public class GetNewsPaperDetailsQuery : IRequest<Result>
    {
        public class GetNewsPaperDetailsQueryHandler : IRequestHandler<GetNewsPaperDetailsQuery, Result>
        {
            private readonly DapperServiceHelper _dapper;
            public GetNewsPaperDetailsQueryHandler(DapperServiceHelper dapper)
            {
                _dapper = dapper;
            }

            public async Task<Result> Handle(GetNewsPaperDetailsQuery request, CancellationToken cancellationToken)
            {
                var result = await _dapper.QueryListAsync<GetNewsPaperDetailsDto>("Usp_GetAllNewsPaperDetails");
                if (result != null)
                {
                    return Result.Success(result);
                }
                return Result.Failure(new Error("BA502"));
            }
        }
    }
}
