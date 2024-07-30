using BookStore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class books
    {
        [Key]
        public int idbooks { get; set; }

        public string? title { get; set; } // Nullable string

        public string? author { get; set; } // Nullable string

        public int price { get; set; }

        public string? description { get; set; } // Nullable string

        public int stock { get; set; }

        public string? photo { get; set; } // Nullable string

        public string? type { get; set; } // Nullable string

        public int pages { get; set; }

        public string? publisher { get; set; } // Nullable string

        public int pub_date { get; set; }

        public string b_photo { get; set; }

        public int fk_sid { get; set; }

        [ForeignKey("fk_sid")]
        public sellers sid { get; set; }

    }
}