using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using E_commerce.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace E_commerce.Background
{
    public class OrderBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<OrderBackgroundWorker> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

        public OrderBackgroundWorker(IServiceProvider provider, ILogger<OrderBackgroundWorker> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OrderBackgroundWorker started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _provider.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDB>();

                        // Process pending orders -> Completed
                        var pending = await db.Orders.Where(o => o.Status == Models.OrderStatus.Pending).Include(o => o.Products).Include(o => o.Customer).ToListAsync(stoppingToken);
                        foreach (var order in pending)
                        {
                            order.Status = Models.OrderStatus.Completed;
                        }
                        if (pending.Any())
                        {
                            await db.SaveChangesAsync(stoppingToken);
                        }

                        // Refresh materialized view (simple: clear and repopulate)
                        db.DashboardOrders.RemoveRange(db.DashboardOrders);
                        await db.SaveChangesAsync(stoppingToken);

                        var dashboard = await db.Orders
                            .Include(o => o.Customer)
                            .Include(o => o.Products)
                            .Select(o => new Models.DashboardOrder { Id = o.Id, CustomerName = o.Customer.Name, ItemCount = o.Products.Sum(p => p.Quantity), Total = o.Products.Sum(p => p.Price * p.Quantity), Status = o.Status.ToString() })
                            .ToListAsync(stoppingToken);

                        if (dashboard.Any())
                        {
                            db.DashboardOrders.AddRange(dashboard);
                            await db.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in background worker");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
