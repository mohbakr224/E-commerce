namespace E_commerce.Models
{
    public class DashboardOrder
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal Price { get; set; }
        public double Total { get; set; }
    }
}
