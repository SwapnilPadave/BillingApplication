using BA.Dtos.CustomerDto;
using BA.Service.Customer;
using BA.Utility.Result;
using MediatR;

namespace BA.Api.Infra.MediatorHandlers.CustomerHandler
{
    public class AddCustomerHandler : IRequestHandler<AddCustomerCommand, Result>
    {
        private readonly ICustomerService _customerService;
        public AddCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Result> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.AddCustomerAsync(request.UserId, request.AddCustomer);
        }
    }
}
