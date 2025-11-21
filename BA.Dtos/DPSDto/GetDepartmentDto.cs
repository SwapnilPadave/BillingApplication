namespace BA.Dtos.DPSDto
{
    public class GetDepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }

    public class GetPositionRoleDto
    {
        public int RoleId { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }

    public class GetShiftDto
    {
        public int ShiftId { get; set; }
        public string ShiftCode { get; set; } = string.Empty;
        public string ShiftTime { get; set; } = string.Empty;
    }
}
