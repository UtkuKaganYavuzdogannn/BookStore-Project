using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class SellerRegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int PhoneNum { get; set; } // Telefon numarası string olarak tanımlanabilir
    }
}