using E_commerce.Data;
using E_commerce.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace E_commerce.Features.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly ApplicationDB _db;

        public CreateOrderCommandHandler(ApplicationDB db)
        {
            _db = db;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Simple validations
            if (request.Items == null || !request.Items.Any())
                throw new ArgumentException("Order must contain at least one item.");

            if (request.Items.Any(i => i.Quantity <= 0))
                throw new ArgumentException("All items must have quantity greater than zero.");

            // Ensure customer exists (basic rule)
            var customer = await _db.Customers.FindAsync(new object[] { request.CustomerId }, cancellationToken);
            if (customer == null)
                throw new ArgumentException("Customer not found.");

            var order = new Order
            {
                CustomerId = request.CustomerId,
            };

            foreach (var item in request.Items)
            {
                order.Products ??= new System.Collections.Generic.List<Products>();
                order.Products.Add(new Products
                {
                    Name = item.Name,
                    Price = item.UnitPrice,
                    Quantity = item.Quantity,
                    order = order
                });
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync(cancellationToken);

            // Invalidate cache for this order if distributed cache is used elsewhere (best-effort)
            // Cache invalidation implemented in request pipeline or after MediatR in controller in later steps if needed.

            return order.Id;
        }
    }
}
