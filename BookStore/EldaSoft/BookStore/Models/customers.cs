using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class customers
    {
        [Key]
        public int cid { get; set; }
        public string Name { get; set; }
        public string c_mail { get; set; }
        public string password { get; set; }
        public int phone_num { get; set; }
        public string cus_city { get; set; }
        public bool IsDeleted { get; set; }  // Soft delete için eklenen alan
    }
}
