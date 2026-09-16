using AutoMapper;
using E_commerce.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Features.Orders.DeleteOrder
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly ApplicationDB _dbcontext;
        private readonly IMapper _mapper;

        public DeleteOrderHandler(ApplicationDB dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
           Models.Order? order =await _dbcontext.Orders.FirstOrDefaultAsync(o=>o.CustomerId == request.CustomerId);
            if (order == null) 
            {
                throw new ArgumentException("the order not found to delete it");
            
            }
            _dbcontext.Orders.Remove(order);
            _dbcontext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
