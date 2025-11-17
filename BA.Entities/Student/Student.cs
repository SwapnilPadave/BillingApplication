using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Student
{
    [Table("Student")]
    public class Student :AuditProperties
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Address { get; set; } = default!;
    }
}
