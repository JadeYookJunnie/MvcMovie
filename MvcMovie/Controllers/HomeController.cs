using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using System.Diagnostics;

namespace MvcMovie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //   public string ISBN { get; set; }
        // public string Title { get; set; }
        // public string Description { get; set; }
        // public string Picture { get; set; } //for imagesource, can change
        // public List<string> Genre { get; set; }
        // public string Author { get; set; }
        // public int Rating { get; set; }
        // public int ReviewCount { get; set; }

        public IActionResult MyUser()
        {
            var user = new MyUserModel("Thorfinn Karlsefni","password");   

            var books = new List<BookModel>{
                new BookModel("123456", "Title", "Description", "~/images/DummyPic.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5, 100),
                new BookModel("123456", "Title", "Description", "~/images/DummyPic.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5, 100),
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

    
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
