using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.JobApp_Shifts
{
    [Table("JobApp_ShiftDetailsMaster")]
    public class JobApp_ShiftDetailsMaster : AuditProperties
    {
        public string ShiftCode { get; set; } = string.Empty;
        public string ShiftTime { get; set; } = string.Empty;
    }
}

