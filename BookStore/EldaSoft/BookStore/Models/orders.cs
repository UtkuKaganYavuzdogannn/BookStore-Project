using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class orders
    {
        [Key]
        public int oid { get; set; } // Siparişin benzersiz kimliği

        public string address { get; set; } // Adres bilgisi

        public int quantity { get; set; } // Adet

        public int total_price { get; set; } // bu kısımda price x quantity deniycem.

        public string recipient { get; set; } // İletilecek kişi

        public DateTime order_time { get; set; } = DateTime.Now; // Siparişin gerçekleştiği zaman

        // Foreign key tanımlamaları
        public int book_seller_id { get; set; } // Kitabın ait olduğu satıcının ID'si

        [ForeignKey("book_seller_id")]
        public sellers sid { get; set; } // Satıcı ile ilişki

        public int customer_id { get; set; } // Kitabı satın alan müşterinin ID'si

        [ForeignKey("customer_id")]
        public customers cid { get; set; } // Müşteri ile ilişki

        public int fk_bookid { get; set; } //bookid fk tutması için.

        [ForeignKey("fk_bookid")]
        public books idbooks { get; set; } // Müşteri ile ilişki


    }

}
