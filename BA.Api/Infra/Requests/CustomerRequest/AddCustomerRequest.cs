namespace BA.Api.Infra.Requests.CustomerRequest
{
    public class AddCustomerRequest
    {
        public string BuildingName { get; set; } = string.Empty;
        public string RoomNo { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
    }
}
