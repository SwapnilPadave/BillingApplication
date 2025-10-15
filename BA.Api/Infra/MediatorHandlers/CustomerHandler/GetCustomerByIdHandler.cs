using BA.Dtos.CustomerDto;
using BA.Entities.Customer;
using BA.Service.Customer;
using BA.Utility.Result;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerHandler
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDetails>
    {
        private readonly ICustomerService _customerService;
        public GetCustomerByIdHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task<CustomerDetails> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetCustomerByIdForConsumApiAsync(request.Id);
        }
    }
}
