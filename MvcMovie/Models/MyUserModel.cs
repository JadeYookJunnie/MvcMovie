namespace MvcMovie.Models
{
    public class MyUserModel : UserModel
    {
        public string Name {get; set;}
        public string Password { get; set; }
        public List<BookModel> Favourites { get; set; } = new List<BookModel>(); 
        public List<BookModel> Wishlist { get; set; } = new List<BookModel>(); 
        public List<UserModel> Friendlist { get; set; } = new List<UserModel>(); 

        public List<ReviewModel> ReviewList { get; set; } = new List<ReviewModel>();

       public MyUserModel(string name, string password)
        {
            Name = name;
            Password = password; 
            CurrentReads = new List<BookModel>(); 
            Favorites = new List<BookModel>();    
        }

        public void addFriend(UserModel user)
        {
            Friendlist.Add(user);
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
