namespace BA.Dtos.EmployeeDto
{
    public class GetEmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Salary { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; } = string.Empty;
        public int ShiftId { get; set; }
        public string ShiftName { get; set; } = string.Empty;
    }
}
