using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class AddBooksViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stok pozitif bir değer olmalıdır.")]
        public int Stock { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }
    }
}
