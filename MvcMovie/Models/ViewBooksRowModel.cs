namespace MvcMovie.Models
{
    public class ViewBooksRowModel
    {
        // Properties with explicit types
        public string Title { get; set; }
        public List<BookModel> Books { get; set; }

        // Constructor to initialize properties
        public ViewBooksRowModel(string title, List<BookModel> books)
        {
            Title = title;
            Books = books;
        }
    }
}