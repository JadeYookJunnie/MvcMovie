using Microsoft.AspNetCore.Mvc;

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

    public ReviewModel(int id, UserModel user, BookModel book, int likeCount, int rating, string review, DateTime date)
        {
            Id = id;
            User = user;
            Book = book;
            LikeCount = likeCount;
            Rating = rating;
            Review = review;
            Date = date;
        }

        public void updateLikes(bool add) { 
            if (add)
            {
                LikeCount++;
            }
            else if (LikeCount > 0)
            {
                LikeCount--;
            }
        }

        public void toggleLike(bool add) {
            if (add)
            {
                LikeCount++;
            }
            else if (LikeCount > 0)
            {
                LikeCount--;
            }
        }
    }
}
