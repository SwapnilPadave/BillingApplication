using BA.Entities.Departments;
using BA.Entities.Shift;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Employees
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Salary { get; set; }
        public int DeptId { get; set; }

        [ForeignKey("DeptId")]
        public Department Department { get; set; }
        public int ShiftId { get; set; }

        [ForeignKey("ShiftId")]
        public Shifts Shift { get; set; }
    }
}
