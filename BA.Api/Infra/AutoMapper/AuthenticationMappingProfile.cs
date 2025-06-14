using AutoMapper;
using BA.Api.Infra.Requests.LoginRequest;
using BA.Dtos.LoginDto;

namespace BA.Api.Infra.AutoMapper
{
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {            
            CreateMap<RegisterUserRequest, RegisterUserDto>().ReverseMap();
        }
    }
}
