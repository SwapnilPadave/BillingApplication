using BA.Entities.Country_State_City;

namespace BA.Dtos.CSCDto
{
    public class GetCountryStateCityDetailsDto
    {
        public List<GetCountryDetailsDto> GetCountryDetailsDto { get; set; } = new List<GetCountryDetailsDto>();
        public List<GetStateDetailsDto> GetStateDetailsDto { get; set; } = new List<GetStateDetailsDto>();
        public List<GetCityDetailsDto> GetCityDetailsDto { get; set; } = new List<GetCityDetailsDto>();
    }
}
