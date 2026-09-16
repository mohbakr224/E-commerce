using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using E_commerce.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using E_commerce.Models;
using Hangfire;

namespace E_commerce.Background
{
    public class OrderBackgroundWorker
    {
        private readonly ApplicationDB _applicationDB;

        public OrderBackgroundWorker(ApplicationDB applicationDB)
        {
            _applicationDB = applicationDB;
        }
        public async Task GetOrderBackgroud()
        {
            var orders=await _applicationDB.Orders.Include(o => o.Customer).Include(o=>o.Products).ToListAsync();
            await _applicationDB.SaveChangesAsync();
            _applicationDB.DashboardOrders.RemoveRange(_applicationDB.DashboardOrders);
            IList<DashboardOrder> dashboards = new List<DashboardOrder>();

            foreach (var o in orders)
            {
                dashboards.Add(new DashboardOrder()
                {
                    CustomerName = o.Customer.Name,
                    Total = o.Status == OrderStatus.Completed ? o.Products.Count : 0,
                    Price = o.Status == OrderStatus.Completed ? (decimal)o.Products.Sum(p => p.Price) : 0m
                });
            }
            _applicationDB.DashboardOrders.AddRange(dashboards);
            _applicationDB.SaveChanges();
            BackgroundJob.Schedule<OrderBackgroundWorker>(x=>x.GetOrderBackgroud(),TimeSpan.FromSeconds(30)
                );
        }
    }
}
