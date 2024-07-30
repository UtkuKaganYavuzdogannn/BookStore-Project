using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class OrderViewModel
    {
        [Required]
        public string Address { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        public string Recipient { get; set; }

        public int BookSellerId { get; set; }
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }

        public DateTime OrderTime { get; set; } 
    }
}
