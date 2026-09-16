using AutoMapper;
using E_commerce.Data;
using E_commerce.Features.Order;
using E_commerce.Features.Order.CreateOrder;
using E_commerce.Models;
using MediatR;

namespace E_commerce.Features.Orders.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly IMapper _mapper;
        private ApplicationDB _Dbcontext;
        public CreateOrderHandler(ApplicationDB applicationDB , IMapper mapper)
        {
            _mapper= mapper;
            _Dbcontext= applicationDB;
        }
        public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
           Models.Order order = new Models.Order()
           {
               CustomerId = request.CustomerId,
           };
            _Dbcontext.Add(order);
          await  _Dbcontext.SaveChangesAsync();
            return _mapper.Map<OrderResponse>(order);

        }
    }
}
