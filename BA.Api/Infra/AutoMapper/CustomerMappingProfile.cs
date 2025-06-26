using AutoMapper;
using BA.Api.Infra.Requests.CustomerRequest;
using BA.Dtos.CustomerDto;

namespace BA.Api.Infra.AutoMapper
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<AddCustomerRequest, AddCustomerDto>().ReverseMap();
        }
    }
}
