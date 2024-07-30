using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class sellers
    {
        [Key]
        public int sid { get; set; }
        public string s_name { get; set; }
        public string s_mail { get; set; }
        public string s_password { get; set; }
        public int s_phoneNum { get; set; }
        public string city { get; set; }
    }
}
