using BA.Entities.Customer;
using MediatR;

namespace BA.Dtos.CustomerDto
{
    public class GetCustomerByIdQuery : IRequest<CustomerDetails>
    {
        public int Id { get; set; }
        public GetCustomerByIdQuery(int id)
        {
            Id = id;
        }
    }
}
