namespace BA.Dtos.EmployeeDto
{
    public class EmployeeBulkUploadExcelDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string DateOfJoining { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int ShiftId { get; set; }
    }
}
