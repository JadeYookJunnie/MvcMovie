using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using System.Diagnostics;

namespace MvcMovie.Controllers
{
    public class HomeController : Controller
    {
<<<<<<< HEAD
        public DynamoController dynamoController = new DynamoController();
        //string current = dynamoController.getReads();
=======

        /* SIGN IN INFORMATION FOR EXISTING ACCOUNT COS I FUCKED UP SIGN UP PROCESS:
         * username: admin
         * password: Adm!n1strator
         */

>>>>>>> dev
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult MyUser()
        {
            var user = new MyUserModel("Thorfinn Karlsefni","password");   

            var books = new List<BookModel>{
                new BookModel("123456", "To Kill a Mockingbird", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5, 100),
                new BookModel("123456", "The Great Gatsby", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5, 100),
                new BookModel("123456", "The Lion, the Witch and the Wardrobe ", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5, 100),
                new BookModel("123456", "Title", "Description", "~/images/bookcover.jpg", new List<string> { "Genre1", "Genre2" }, "Author", 5, 100),
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
