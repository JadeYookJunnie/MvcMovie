namespace MvcMovie.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public BookModel Book { get; set; }
        public int LikeCount { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime Date { get; set; }

        public void init(UserModel user, BookModel book, int rating)
        {

        }

        public void updateLikes(bool add) { }
    }
}
