using BA.Entities.Employees;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Shift
{
    [Table("Shifts")]
    public class Shifts
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Employee> Employees { get; set; }
    }
}
