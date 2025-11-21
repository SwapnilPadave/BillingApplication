using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.JobApp_PositionRole
{
    [Table("JobApp_PositionRoleDetailMaster")]
    public class JobApp_PositionRoleDetailMaster : AuditProperties
    {
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}
