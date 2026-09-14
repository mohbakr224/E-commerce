namespace E_commerce.Models
{
    public class DashboardOrder
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int ItemCount { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
    }
}
