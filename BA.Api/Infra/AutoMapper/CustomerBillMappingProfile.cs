using AutoMapper;
using BA.Api.Infra.Requests.BillRequest;
using BA.Dtos.BillDto;

namespace BA.Api.Infra.AutoMapper
{
    public class CustomerBillMappingProfile : Profile
    {
        public CustomerBillMappingProfile()
        {
            CreateMap<CalculateTotalAmountRequest, CalculateTotalAmountDto>().ReverseMap();
            CreateMap<NewsPaperIdsAndAmountRequest, NewsPaperIdsAndAmountDto>();
        }
    }
}
