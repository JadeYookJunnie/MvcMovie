using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using System.Diagnostics;
using MvcMovie.Services;

namespace MvcMovie.Controllers
{
    public class HomeController : Controller
    {

        /* SIGN IN INFORMATION FOR EXISTING ACCOUNT COS I FUCKED UP SIGN UP PROCESS:
         * username: admin
         * password: Adm!n1strator
         */

        private readonly ILogger<HomeController> _logger;
        private readonly GoogleBooksService _googleBooksService;

        public HomeController(ILogger<HomeController> logger, GoogleBooksService googleBooksService)
        {
            _logger = logger;
            _googleBooksService = googleBooksService;
        }

        public IActionResult Index()
        {
            return View();
        }
         public async Task<IActionResult> BrowseArea()
        {        // Fetch books from Google Books API
                var books = await _googleBooksService.SearchAllBooksAsync(null);
                Console.WriteLine("Adding book " + books);
                return View(books);
    
        }
        public IActionResult MyUser()
{
    var user = new MyUserModel("Thorfinn Karlsefni", "password");

    var books = new List<BookModel>{
        new BookModel("123456", "To Kill a Mockingbird", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Harper Lee", 5, new DateTime(1960, 7, 11)),
        new BookModel("123456", "The Great Gatsby", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "F. Scott Fitzgerald", 5, new DateTime(1925, 4, 10)),
        new BookModel("123456", "The Lion, the Witch and the Wardrobe", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "C.S. Lewis", 5, new DateTime(1950, 10, 16)),
        new BookModel("123456", "Title", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5, new DateTime(1960, 7, 11)),
    };

    var users = new List<UserModel>
    {
        new UserModel { UserId = "1", Name = "John Doe" },
        new UserModel { UserId = "2", Name = "Jane Smith" },
        new UserModel { UserId = "3", Name = "Alice Johnson" },
        new UserModel { UserId = "4", Name = "Bob Brown" },
        new UserModel { UserId = "5", Name = "Charlie Davis" }
    };

    foreach (var book in books)
    { //ONCE WE HAVE CRUD THESE MUST BE CHANGED
        Console.WriteLine("Adding book " + book.ISBN);

        // Add the book to the user's current reads and favorites
        user.addBookToCurrentReads(book);
        user.addBookToFavorites(book);

        // Create 5 reviews per book with 2-6 sentence dummy content
        var dummyReviews = new List<string>
        {
            $"A gripping tale of morality and humanity. The characters are so well-developed that you can't help but empathize with them. A true classic that resonates even today.",
            $"The story was captivating, but the pacing felt slow at times. However, the themes explored were deep and thought-provoking. A must-read for any literature enthusiast.",
            $"An enchanting read that transports you to another world. The imagery and descriptive language are top-notch. The ending was both satisfying and bittersweet.",
            $"I enjoyed the book overall, but some parts were predictable. The author's style is unique and compelling, making it hard to put the book down. The story stayed with me long after I finished it.",
            $"A masterpiece in every sense of the word. The narrative is beautifully crafted, and the dialogue is sharp and insightful. It's no wonder this book has stood the test of time."
        };

        for (int i = 0; i < 5; i++)
        {
            var review = new ReviewModel(
                id: i + 1, 
                user: users[i % users.Count], // Cycle through the list of users
                book: book,
                likeCount: i * 2, // Example like count
                rating: (i % 5) + 1, // Example rating from 1 to 5
                review: dummyReviews[i], // Using one of the dummy reviews
                date: DateTime.Now.AddDays(-i) // Recent dates
            );

            book.addReview(review); // Assuming the BookModel has an addReview method
        }
    }

    return View(user);
}
        
        public IActionResult Book()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Review()
        {
            return View();
        }


        private List<ReviewModel> GetDummyReviews(List<BookModel> currentReads)
        {
            var user1 = new UserModel { UserId = "1", Name = "John Doe" };
            var user2 = new UserModel { UserId = "2", Name = "Jane Smith" };
            var user3 = new UserModel { UserId = "3", Name = "Alice Johnson" };
            var user4 = new UserModel { UserId = "4", Name = "Bob Brown" };
            var user5 = new UserModel { UserId = "5", Name = "Charlie Davis" };

            var reviews = new List<ReviewModel>();

            foreach (var book in currentReads)
            {
                reviews.AddRange(new List<ReviewModel>
                {
                    new ReviewModel(1, user1, book, 10, 5, $"A masterpiece of {book.Title}.", DateTime.Now.AddDays(-1)),
                    new ReviewModel(2, user2, book, 8, 4, $"Very impactful, but a bit slow at times in {book.Title}.", DateTime.Now.AddDays(-2)),
                    new ReviewModel(3, user3, book, 15, 5, $"Incredible story, beautifully written in {book.Title}.", DateTime.Now.AddDays(-3)),
                    new ReviewModel(4, user4, book, 2, 3, $"Good, but not my favorite from {book.Title}.", DateTime.Now.AddDays(-4)),
                    new ReviewModel(5, user5, book, 7, 4, $"{book.Title} is a must-read for everyone.", DateTime.Now.AddDays(-5)),
                });
            }

            return reviews;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
