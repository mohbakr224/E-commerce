namespace E_commerce.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Double Price { get; set; }
        public int OrderId { get; set; }
        public Order order { get; set; }
    }
}
