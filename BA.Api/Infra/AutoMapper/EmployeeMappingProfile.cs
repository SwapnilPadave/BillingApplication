using AutoMapper;
using BA.Api.Infra.Requests.EmployeeRequest;
using BA.Dtos.EmployeeDto;

namespace BA.Api.Infra.AutoMapper
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<AddEmployeeRequest, AddEmployeeDetailsDto>().ReverseMap();
            CreateMap<UpdateEmployeeRequest, UpdateEmployeeDetailsDto>().ReverseMap();
        }
    }
}
