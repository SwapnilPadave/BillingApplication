using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Customer
{
    [Table("CustomerDetails")]
    public class CustomerDetails : AuditProperties
    {
        public string BuildingName { get; set; } = string.Empty;
        public string RoomNo { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
    }
}
