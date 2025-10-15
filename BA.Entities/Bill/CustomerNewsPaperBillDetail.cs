using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Bill
{
    [Table("CustomerNewsPaperBillDetail")]
    public class CustomerNewsPaperBillDetail
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int NewsPaperId { get; set; }
        public int NormalDays { get; set; }
        public int Sundays { get; set; }
        public int Saturday { get; set; }
        public int SpecialDays { get; set; }
        public int TotalDays { get; set; }
        public decimal NormalDayAmount { get; set; }
        public decimal SundayAmount { get; set; }
        public decimal SaturdayAmount { get; set; }
        public decimal SpecialDayAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
