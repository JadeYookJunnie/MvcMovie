namespace MvcMovie.Models
{
    public class BookModel
    {   
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public List<string> Genre { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public DateTime PublishDate { get; set; }
        public int ReviewCount { get; set; }
        public List<ReviewModel> BookReviews { get; set; }

        public BookModel(string ISBN, string Title, string Description, string Picture, List<string> Genre, string Author, int Rating)
        {
            this.ISBN = ISBN;
            this.Title = Title;
            this.Description = Description;
            this.Picture = Picture;
            this.Genre = Genre;
            this.Author = Author;
            this.Rating = Rating;
            this.ReviewCount = 0;
            this.BookReviews = new List<ReviewModel>();
        }

        public void setFavorite(BookModel book)
        {   
       
        }

        public void addWishList(BookModel book)
        {
            
        }
        
        public void addReview(ReviewModel review)
        {  
            BookReviews.Add(review);
        }

        public void RemoveReview(ReviewModel review)
        {  
            BookReviews.Remove(review);
        }
    }
}
