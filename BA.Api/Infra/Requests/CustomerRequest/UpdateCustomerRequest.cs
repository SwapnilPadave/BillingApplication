namespace BA.Api.Infra.Requests.CustomerRequest
{
    public class UpdateCustomerRequest
    {
        public string BuildingName { get; set; } = string.Empty;
        public string RoomNo { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
    }
}
