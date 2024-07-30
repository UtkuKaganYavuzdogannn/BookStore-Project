namespace BookStore.Models
{
    public class BooksUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string Photo { get; set; }
    }
}
