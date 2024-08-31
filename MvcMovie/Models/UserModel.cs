namespace MvcMovie.Models
{
    public class UserModel
    {
        public String UserId { get; set; }
        public string Name { get; set; }
        public List<BookModel> Favorites { get; set; }
        public List<BookModel> Wishlist { get; set; }
        public List<BookModel> CurrentReads { get; set; }
        public List<UserModel> Friends { get; set; }
        public List<ReviewModel> MyReviews { get; set; }


    }
}