using AutoMapper;
using BA.Api.Infra.Requests.StudentRequest;
using BA.Dtos.StudentDto;

namespace BA.Api.Infra.AutoMapper
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<AddStudentRequest, AddStudentDetailsDto>().ReverseMap();
        }
    }
}
