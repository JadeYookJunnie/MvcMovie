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
    public async Task<IActionResult> MyUser()
    {

        // need to be able to pass in the username of the logged in user
        string username = "abbie";

        // constructor of user model gets the data and populates into the fields
        var user = new MyUserModel(username);
        //await user.GetDataAsync(username);
        await user.IdsToBookModel(_googleBooksService);
        
        // Console.WriteLine("Current reads: " + user.CurrentReads.Count);
        // Console.WriteLine("Favourties: " + user.Favourites.Count);


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
