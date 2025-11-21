namespace BA.Dtos.DPSDto
{
    public class GetDepartmentRoleShiftDetailsDto
    {
        public List<GetDepartmentDto> Departments { get; set; } = new List<GetDepartmentDto>();
        public List<GetPositionRoleDto> PositionRoles { get; set; } = new List<GetPositionRoleDto>();
        public List<GetShiftDto> Shifts { get; set; } = new List<GetShiftDto>();
    }
}
