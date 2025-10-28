namespace BA.Dtos.BillDto
{
    public class GenerateBillDetailsPdfDto
    {
        public string FileName { get; set; } = string.Empty;
        public string Base64String { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
    }
}
