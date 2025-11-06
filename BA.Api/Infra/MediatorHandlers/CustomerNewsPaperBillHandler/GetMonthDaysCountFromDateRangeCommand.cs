using BA.Database;
using BA.Dtos.BillDto;
using BA.Utility.Content;
using BA.Utility.Result;
using Dapper;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerNewsPaperBillHandler
{
    public class GetMonthDaysCountFromDateRangeCommand : IRequest<Result>
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int SpecialDays { get; set; } = 0;

        public class GetMonthDaysCountFromDateRangeCommandHandler : IRequestHandler<GetMonthDaysCountFromDateRangeCommand, Result>
        {
            private readonly DapperServiceHelper _dapperServiceHelper;
            public GetMonthDaysCountFromDateRangeCommandHandler(DapperServiceHelper dapperServiceHelper)
            {
                _dapperServiceHelper = dapperServiceHelper;
            }

            public async Task<Result> Handle(GetMonthDaysCountFromDateRangeCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@FromDate", request.FromDate);
                    param.Add("@ToDate", request.ToDate);
                    param.Add("@SpecialDays", request.SpecialDays);

                    var result = await _dapperServiceHelper.QueryFirstOrDefaultAsync<GetMonthDaysDetailsDto>(@"Select * From fn_GetSatSunNormalDaysCount (@FromDate, @ToDate, @SpecialDays)", param);

                    return Result.Success(result!);
                }
                catch (Exception)
                {
                    return Result.Failure(new Error("BA501"));
                }
            }
        }
    }
}
