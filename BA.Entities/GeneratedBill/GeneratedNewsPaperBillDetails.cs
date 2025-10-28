using System.ComponentModel.DataAnnotations.Schema;

namespace BA.Entities.GeneratedBill
{
    [Table("GeneratedNewsPaperBillDetails")]
    public class GeneratedNewsPaperBillDetails : AuditProperties
    {
        public int CustomerId { get; set; }
        public int BillId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
    }
}
