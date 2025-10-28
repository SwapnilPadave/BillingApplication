using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.Bill
{
    [Table("CustomerBillDetails")]
    public class CustomerBillDetails : AuditProperties
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string NewsPaperIds { get; set; } = string.Empty;
        public DateTime? FromDate { get; set; } = DateTime.Now;
        public DateTime? ToDate { get; set; } = DateTime.Now;
        public int TotalDays { get; set; }
        public decimal Amount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsBillPaid { get; set; }
    }
}
