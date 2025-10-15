using BA.Utility.Result;
using MediatR;

namespace BA.Dtos.CustomerDto
{
    public class AddCustomerCommand : IRequest<Result>
    {
        public int UserId { get; set; }
        public AddCustomerDto AddCustomer { get; set; } = new AddCustomerDto();
        public AddCustomerCommand(AddCustomerDto addCustomer, int userId)
        {
            AddCustomer = addCustomer;
            UserId = userId;
        }
    }
}
