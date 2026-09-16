using MediatR;

namespace E_commerce.Features.Orders.DeleteOrder
{
    public class DeleteOrderCommand:IRequest
    {
        public int CustomerId { get; set; }
    }
}
