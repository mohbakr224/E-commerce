using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using E_commerce.Data;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Features.Orders.GetOrders
{
    public class GetOrdersQuery : IRequest<List<OrderListDto>> { }

    public class OrderListDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int ItemCount { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderListDto>>
    {
        private readonly ApplicationDB _db;

        public GetOrdersQueryHandler(ApplicationDB db)
        {
            _db = db;
        }

        public async Task<List<OrderListDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .ToListAsync(cancellationToken);

            return orders.Select(o => new OrderListDto
            {
                Id = o.Id,
                CustomerName = o.Customer?.Name,
                ItemCount = o.Products.Sum(p => p.Quantity),
                Total = o.Products.Sum(p => p.Price * p.Quantity),
                Status = o.Status.ToString()
            }).ToList();
        }
    }
}
