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

        public BookModel(string ISBN, string Title, string Description, string Picture, List<string> Genre, string Author, int Rating){
            this.ISBN = ISBN;
            this.Title = Title;
            this.Description = Description;
            this.Picture = Picture;
            this.Genre = Genre;
            this.Author = Author;
            this.Rating = Rating;
            this.ReviewCount = ReviewCount;
        }

        public void setFavorite()
        {

        }

        public void addWishList() { }
        
        public void addReview() { }
    }
}
