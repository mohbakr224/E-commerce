using E_commerce.Models;
using MediatR;

namespace E_commerce.Features.Order.CreateOrder
{
    public class CreateOrderCommand:IRequest<OrderResponse>
    {
        public int CustomerId { get; set; }
    }
}
