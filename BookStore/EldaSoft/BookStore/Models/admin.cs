using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public class admin
    {
        public int AdminId { get; set; }
        public string AdminCode { get; set; }
        public string Password { get; set; }
    }
}
