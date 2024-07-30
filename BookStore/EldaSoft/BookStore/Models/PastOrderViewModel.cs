namespace BookStore.Models
{
    public class PastOrderViewModel
    {
        public int OrderId { get; set; }
        public string BookTitle { get; set; }
        public string SellerName { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderTime { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
