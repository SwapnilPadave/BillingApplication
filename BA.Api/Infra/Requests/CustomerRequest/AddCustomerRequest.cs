using System.ComponentModel.DataAnnotations;

namespace BA.Api.Infra.Requests.CustomerRequest
{
    public class AddCustomerRequest
    {
        [MaxLength(3, ErrorMessage ="Max 3 char expected.")]
        public string BuildingName { get; set; } = string.Empty;
        public string RoomNo { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
    }
}
