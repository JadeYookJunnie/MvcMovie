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

        public List<BookModel> getFavorites()
        {
            return new List<BookModel>();
        }

        public List<BookModel> getWishlist()
        {
            return new List<BookModel>();
        }

        public List<UserModel> getFriends()
        {
            return new List<UserModel>();
        }

        public List<ReviewModel> getReviewModels() { 
            return new List<ReviewModel>();
        }

        public List<BookModel> getCurrentReads() {
            return new List<BookModel>();
        }


    }
}