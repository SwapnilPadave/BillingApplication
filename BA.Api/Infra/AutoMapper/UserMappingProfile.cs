using AutoMapper;
using BA.Api.Infra.Requests.UserRequests;
using BA.Dtos.UserDtos;

namespace BA.Api.Infra.AutoMapper
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<AddUserRequest, AddUserDto>()
                .ReverseMap();

            CreateMap<UpdateUserRequest, UpdateUserDto>()
                .ReverseMap();
        }
    }
}
