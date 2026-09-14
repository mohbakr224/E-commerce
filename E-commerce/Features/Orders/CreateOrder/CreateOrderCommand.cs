using MediatR;
using System.Collections.Generic;

namespace E_commerce.Features.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
