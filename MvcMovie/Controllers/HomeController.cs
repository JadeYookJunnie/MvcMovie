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
                return View(books);
    
        }
        public IActionResult MyUser()
        {
            var user = new MyUserModel("Thorfinn Karlsefni","password");   

            var books = new List<BookModel>{
                new BookModel("testingID-1","123456", "To Kill a Mockingbird", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5),
                new BookModel("testingID-2","123456", "The Great Gatsby", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5),
                new BookModel("testingID-3","123456", "The Lion, the Witch and the Wardrobe ", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5),
                new BookModel("testingID-5","123456", "Title", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5),
            };
            foreach (var book in books)
            {
                user.addBookToCurrentReads(book);
                user.addBookToFavorites(book);
                
            };
            
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
            var reviews = GetDummyReviews();
            return View(reviews);
        }


        private List<ReviewModel> GetDummyReviews()
        {
            var user1 = new UserModel { UserId = "1", Name = "John Doe" };
            var user2 = new UserModel { UserId = "2", Name = "Jane Smith" };
            var user3 = new UserModel { UserId = "3", Name = "Alice Johnson" };
            var user4 = new UserModel { UserId = "4", Name = "Bob Brown" };
            var user5 = new UserModel { UserId = "5", Name = "Charlie Davis" };

            var book1 = new BookModel("testingID-1", "123456", "To Kill a Mockingbird", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Harper Lee", 5);
            var book2 = new BookModel("testingID-2", "123456", "The Great Gatsby", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "F. Scott Fitzgerald", 5);

            return new List<ReviewModel>
            {
                // Reviews for "To Kill a Mockingbird"
                new ReviewModel(1, user1, book1, 10, 5, "A masterpiece. Timeless and powerful.", DateTime.Now.AddDays(-1)),
                new ReviewModel(2, user2, book1, 8, 4, "Very impactful, but a bit slow at times.", DateTime.Now.AddDays(-2)),
                new ReviewModel(3, user3, book1, 15, 5, "Incredible story, beautifully written.", DateTime.Now.AddDays(-3)),
                new ReviewModel(4, user4, book1, 2, 3, "Good, but not my favorite.", DateTime.Now.AddDays(-4)),
                new ReviewModel(5, user5, book1, 7, 4, "A must-read for everyone.", DateTime.Now.AddDays(-5)),

                // Reviews for "The Great Gatsby"
                new ReviewModel(6, user1, book2, 12, 5, "A haunting story of lost dreams.", DateTime.Now.AddDays(-6)),
                new ReviewModel(7, user2, book2, 5, 4, "Beautifully written, but somewhat sad.", DateTime.Now.AddDays(-7)),
                new ReviewModel(8, user3, book2, 9, 5, "An iconic novel. Loved it.", DateTime.Now.AddDays(-8)),
                new ReviewModel(9, user4, book2, 3, 3, "Not as good as I expected.", DateTime.Now.AddDays(-9)),
                new ReviewModel(10, user5, book2, 4, 4, "Great book, but a bit overrated.", DateTime.Now.AddDays(-10)),
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
