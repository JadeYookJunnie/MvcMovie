namespace MvcMovie.Models
{
    public class MyUserModel : UserModel
    {
        public string Password { get; set; }

        public void addFriend(UserModel user)
        {

        }

        public void removeFriend(UserModel user) { }

        public void addBookToWishlist(BookModel book) { }
        public void addBookToFavorites(BookModel book) { }
        public void addBookToCurrentReads(BookModel book) { }
        public void removeBookFromWishlist(BookModel book) { }
        public void removeBookFromFav(BookModel book) { }
        public void removeBookFromCurrReads(BookModel book) { }


    }
}
