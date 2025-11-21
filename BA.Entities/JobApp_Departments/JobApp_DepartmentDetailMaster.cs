using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.JobApp_Departments
{
    [Table("JobApp_DepartmentDetailMaster")]
    public class JobApp_DepartmentDetailMaster : AuditProperties
    {
        public string Name { get; set; } = string.Empty;
    }
}
