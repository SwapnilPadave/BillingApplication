namespace BA.Dtos.EmployeeDto
{
    public class GetEmployeeListForExcelReport
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string MobileNumber { get; set; } = default!;
        public string DateOfBirth { get; set; } = default!;
        public string DateOfJoining { get; set; } = default!;
        public int Age { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string CountryName { get; set; } = default!;
        public string StateName { get; set; } = default!;
        public string CityName { get; set; } = default!;
        public string DeptName { get; set; } = default!;
        public string RoleName { get; set; } = default!;
        public string ShiftCode { get; set; } = default!;
        public string ShiftTime { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
