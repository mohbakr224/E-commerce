namespace E_commerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public ICollection<Products> Products { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
