namespace BA.Dtos.CSCDto
{
    public class GetCountryDetailsDto
    {
        public int Id { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }

    public class GetStateDetailsDto
    {
        public int Id { get; set; }
        public string StateName { get; set; } = string.Empty;
        public int CountryId { get; set; }
    }

    public class GetCityDetailsDto
    {
        public int Id { get; set; }
        public string CityName { get; set; } = string.Empty;
    }
}
