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
        private static List<ReviewModel> _reviews = new List<ReviewModel>(); //CHANGE THIS WHEN APIS ARE IMPLEMENTED

        public HomeController(ILogger<HomeController> logger, GoogleBooksService googleBooksService)
        {
            _logger = logger;
            _googleBooksService = googleBooksService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> BrowseArea(string query)
        {        // Fetch books from Google Books API
                var books = await _googleBooksService.SearchAllBooksAsync(query);
                Console.WriteLine("Adding book " + books);
                Console.WriteLine("Adding book " + query);
                return View(books);
    
        }
    public IActionResult MyUser()
    {
        var user = new MyUserModel("Thorfinn Karlsefni", "password");

        var books = new List<BookModel>{
            new BookModel("123456", "To Kill a Mockingbird", "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Harper Lee", 5, new DateTime(1960, 7, 11)),
            new BookModel("123456", "The Great Gatsby", "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "F. Scott Fitzgerald", 5, new DateTime(1925, 4, 10)),
            new BookModel("123456", "The Lion, the Witch and the Wardrobe", "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "C.S. Lewis", 5, new DateTime(1950, 10, 16)),
            new BookModel("123456", "Title", "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5, new DateTime(1960, 7, 11)),
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
        { 
            user.addBookToCurrentReads(book);
            user.addBookToFavorites(book);

            for (int i = 0; i < 5; i++)
            {
                var review = new ReviewModel(
                    id: _reviews.Count + 1, 
                    user: users[i % users.Count], 
                    book: book,
                    likeCount: i * 2, 
                    rating: (i % 5) + 1, 
                    review: $"Sample review for {book.Title}.", 
                    date: DateTime.Now.AddDays(-i) 
                );

                _reviews.Add(review);
                book.addReview(review);
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


    [HttpPost]
    public IActionResult LikeReview(int id)
    {
        // Find the review by its ID in your in-memory list
        var review = _reviews.FirstOrDefault(r => r.Id == id);

        if (review != null)
        {
            review.updateLikes(true); 
            return Json(new { success = true, newLikeCount = review.LikeCount });
        }

        return Json(new { success = false });
    }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
