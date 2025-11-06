using BA.Database;
using BA.Dtos.NewsPaperDto;
using BA.Utility.Content;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.NewsPaperHandler
{
    public class GetNewsPaperDetailsByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }

        public class GetNewsPaperDetailsByIdQueryHandler : IRequestHandler<GetNewsPaperDetailsByIdQuery, Result>
        {
            private readonly DapperServiceHelper _dapper;
            public GetNewsPaperDetailsByIdQueryHandler(DapperServiceHelper dapper)
            {
                _dapper = dapper;
            }
            public async Task<Result> Handle(GetNewsPaperDetailsByIdQuery request, CancellationToken cancellationToken)
            {
                var param = new DynamicParameters();
                param.Add("@Id", request.Id);

                var result = await _dapper.QueryFirstOrDefaultAsync<GetNewsPaperDetailsDto>("Usp_GetNewsPaperDetailsById", param);
                if (result != null)
                {
                    return Result.Success(result);
                }
                return Result.Failure(new Error("BA502"));
            }
        }
    }
}
