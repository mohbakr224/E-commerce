using AutoMapper;
using E_commerce.Data;
using E_commerce.Features.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_commerce.Features.Orders.GetOrderByid
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse>
    {
        public readonly IDistributedCache _cache;
        public readonly ApplicationDB _db;
        public readonly IMapper _mapper;

        public GetOrderByIdHandler(IDistributedCache cache, ApplicationDB db,IMapper mapper)
        {
            _cache = cache;
            _db = db;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            string cachekey = $"Orders:{request.OrderId}";
            string? cacheorder=await _cache.GetStringAsync(cachekey,cancellationToken);
            if (cacheorder != null)
            {
                return JsonSerializer.Deserialize<OrderResponse>(cacheorder);
            }
             var order =_db.Orders.AsNoTracking().FirstOrDefaultAsync(o=>o.Id ==request.OrderId);
            await _cache.SetStringAsync(cachekey,JsonSerializer.Serialize(_mapper.Map<OrderResponse>(order)));
            return _mapper.Map<OrderResponse>(order);

        }
    }
}
