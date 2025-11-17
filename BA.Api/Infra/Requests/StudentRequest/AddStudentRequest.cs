namespace BA.Api.Infra.Requests.StudentRequest
{
    public class AddStudentRequest
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Address { get; set; } = default!;
    }
}
