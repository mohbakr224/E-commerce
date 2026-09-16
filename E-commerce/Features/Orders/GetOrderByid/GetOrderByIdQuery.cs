using E_commerce.Features.Order;
using MediatR;

namespace E_commerce.Features.Orders.GetOrderByid
{
    public class GetOrderByIdQuery:IRequest<OrderResponse>
    {
        public int OrderId { get; set; }
    }
}
