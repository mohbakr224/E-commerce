namespace E_commerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public ICollection<Products> Products { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }  
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }

    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Cancelled = 3
    }
}
