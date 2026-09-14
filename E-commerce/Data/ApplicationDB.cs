using E_commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Data
{
    public class ApplicationDB :DbContext
    {
        public ApplicationDB(DbContextOptions<ApplicationDB> options):base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Products>Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DashboardOrder> DashboardOrders { get; set; }
        protected  override void  OnModelCreating(ModelBuilder modelBuilder) 
        {

            modelBuilder.Entity<Customer>()
                  .HasKey(c => c.Id);
            modelBuilder.Entity<Customer>()
                .Property(c => c.Email);
            modelBuilder.Entity<Customer>()
                .Property(c=>c.Name);

            modelBuilder.Entity<Products>()
              .HasKey(c=>c.Id);
            modelBuilder.Entity<Products>()
                .Property(c=>c.Name);
            modelBuilder.Entity<Products>()
                .Property(p => p.Price);
            modelBuilder.Entity<Products>()
                .Property(p => p.Quantity);
            modelBuilder.Entity<Products>()
                .HasOne(p=>p.order).WithMany(o=>o.Products).HasForeignKey(P=>P.OrderId);

            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);
            modelBuilder.Entity<Order>().
                HasOne(o => o.Customer).WithMany(o => o.Orders).HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<DashboardOrder>().HasKey(d => d.Id);
            modelBuilder.Entity<DashboardOrder>().Property(d => d.CustomerName);
            modelBuilder.Entity<DashboardOrder>().Property(d => d.ItemCount);
            modelBuilder.Entity<DashboardOrder>().Property(d => d.Total);
            modelBuilder.Entity<DashboardOrder>().Property(d => d.Status);
        }

    }
}
