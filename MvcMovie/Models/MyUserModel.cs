namespace MvcMovie.Models
{
    public class MyUserModel : UserModel
    {
        public string Name {get; set;}
        public string Password { get; set; }
        private List<BookModel> Favourites { get; set; } = new List<BookModel>(); 
        private List<BookModel> Wishlist { get; set; } = new List<BookModel>(); 
        private List<UserModel> Friendlist { get; set; } = new List<UserModel>(); 

        public List<ReviewModel> ReviewList { get; set; } = new List<ReviewModel>();

       public MyUserModel(string name, string password)
        {
            Name = name; // Use parameter name
            Password = password; // Use parameter password
        }

        public void addFriend(UserModel user)
        {

        }

        public void removeFriend(UserModel user) { }

        public void addBookToWishlist(BookModel book) 
        {
            Wishlist.Add(book);
        }
        public void addBookToFavorites(BookModel book) 
        { 
            Favourites.Add(book);
        }
        public void addBookToCurrentReads(BookModel book) 
        {
            CurrentReads.Add(book);
        }
        public void removeBookFromWishlist(BookModel book) 
        { 
            Wishlist.Remove(book);
        }
        public void removeBookFromFav(BookModel book) 
        { 
            Favourites.Remove(book);
        }
        public void removeBookFromCurrReads(BookModel book) 
        {
            CurrentReads.Remove(book);
        }


    }
}
