using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using E_commerce.Data;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Features.Orders.GetOrder
{
    public class GetOrderQuery : IRequest<OrderDto>
    {
        public int Id { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int ItemCount { get; set; }
        public double Total { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto>
    {
        private readonly ApplicationDB _db;

        public GetOrderQueryHandler(ApplicationDB db)
        {
            _db = db;
        }

        public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null) return null;

            // Aggregate products by name and price to compute quantities
            var items = order.Products
                .GroupBy(p => new { p.Name, p.Price })
                .Select(g => new OrderItemDto
                {
                    Name = g.Key.Name,
                    UnitPrice = g.Key.Price,
                    Quantity = g.Count()
                })
                .ToList();

            var total = order.Products.Sum(p => p.Price);

            return new OrderDto
            {
                Id = order.Id,
                CustomerName = order.Customer?.Name,
                ItemCount = order.Products.Count,
                Total = total,
                Items = items
            };
        }
    }
}
