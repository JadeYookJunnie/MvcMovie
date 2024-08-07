namespace MvcMovie.Models
{
    public class BookModel
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; } //for imagesource, can change
        public List<string> Genre { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public int ReviewCount { get; set; }

        public void setFavorite()
        {

        }

        public void addWishList() { }
        public void addReview() { }
    }
}
