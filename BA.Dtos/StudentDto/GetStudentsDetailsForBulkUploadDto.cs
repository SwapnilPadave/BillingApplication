namespace BA.Dtos.StudentDto
{
    public class GetStudentsDetailsForBulkUploadDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public List<string> Errors { get; set; } = new();
    }
}
