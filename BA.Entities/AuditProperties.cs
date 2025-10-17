using System.ComponentModel.DataAnnotations;

namespace BA.Entities
{
    public class AuditProperties
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
