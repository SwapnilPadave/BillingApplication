using AutoMapper;
using BA.Api.Infra.Requests.NewsPaperRequests;
using BA.Dtos.NewsPaperDto;

namespace BA.Api.Infra.AutoMapper
{
    public class NewsPaperMappingProfile : Profile
    {
        public NewsPaperMappingProfile()
        {
            CreateMap<AddNewsPaperRequest, AddNewsPaperDto>().ReverseMap();
            CreateMap<UpdateNewsPaperRequest, UpdateNewsPaperDto>().ReverseMap();
        }
    }
}
