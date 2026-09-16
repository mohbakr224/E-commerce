using E_commerce.Models;

namespace E_commerce.Features.Order
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderResponse
    {
        public int Id { get; set; }
        public ICollection<OrderProduct> Products { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

    }
}
