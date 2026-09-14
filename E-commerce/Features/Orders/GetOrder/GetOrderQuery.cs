using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using E_commerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

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
        private readonly IDistributedCache _cache;

        public GetOrderQueryHandler(ApplicationDB db, IDistributedCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"order:{request.Id}";
            var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<OrderDto>(cached);
            }

            var order = await _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null) return null;

            // Map items using Quantity
            var items = order.Products
                .GroupBy(p => new { p.Name, p.Price })
                .Select(g => new OrderItemDto
                {
                    Name = g.Key.Name,
                    UnitPrice = g.Key.Price,
                    Quantity = g.Sum(p => p.Quantity)
                })
                .ToList();

            var total = order.Products.Sum(p => p.Price * p.Quantity);

            var dto = new OrderDto
            {
                Id = order.Id,
                CustomerName = order.Customer?.Name,
                ItemCount = order.Products.Sum(p => p.Quantity),
                Total = total,
                Items = items
            };

            var serialized = JsonSerializer.Serialize(dto);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = System.TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKey, serialized, options, cancellationToken);

            return dto;
        }
    }
}
