using BA.Database;
using BA.Dtos.CSCDto;
using BA.Utility.Result;
using Dapper;

namespace BA.Service.CountryStateCity
{
    public interface ICountryStateCityService
    {
        Task<Result> GetCountryStateCityDetails(int? countryId, int? stateId);
    }

    public class CountryStateCityService : ICountryStateCityService
    {
        private readonly DapperServiceHelper _dapper;
        public CountryStateCityService(DapperServiceHelper dapper)
        {
            _dapper = dapper;
        }

        public async Task<Result> GetCountryStateCityDetails(int? countryId, int? stateId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CountryId", countryId);
                param.Add("@StateId", stateId);

                var data = new GetCountryStateCityDetailsDto();
                var result = await _dapper.QueryMultipleAsync("Usp_GetCountryStateCityDetails", param, multi =>
                {
                    var countryList = multi.Read<GetCountryDetailsDto>().ToList();
                    var stateList = multi.Read<GetStateDetailsDto>().ToList();
                    var cityList = multi.Read<GetCityDetailsDto>().ToList();

                    data.GetCountryDetailsDto = countryList.ToList();
                    data.GetStateDetailsDto = stateList.ToList();
                    data.GetCityDetailsDto = cityList.ToList();

                    return data;
                });

                return Result.Success(data);
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ex.Message));
            }
        }
    }
}
