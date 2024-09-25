using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using System.Diagnostics;
using MvcMovie.Services;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon;
using System.Globalization;

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

        public static async Task<bool> ValidReview(string bookID, string user)
        {
            string tableName = "BR_Reviews";

            DateTime date = DateTime.Today;
            string dateStr = date.Year.ToString() + date.Month.ToString("D2") + date.Day.ToString("D2");

            var key = new Dictionary<string, AttributeValue>
            {
                ["book"] = new AttributeValue { S = bookID },
                ["reviewID"] = new AttributeValue { S = user + dateStr },
            };

            var request = new GetItemRequest
            {
                Key = key,
                TableName = tableName,
            };

            AmazonDynamoDBClient dbClient = dynamoDBConnect();
            var response = await dbClient.GetItemAsync(request);

            if (response.Item == null)
            {
                return false;
            }
            return true;
        }

        public static AmazonDynamoDBClient dynamoDBConnect()
        {
            string accessKey = Environment.GetEnvironmentVariable("DB_ACCESS_KEY");
            string secretKey = Environment.GetEnvironmentVariable("DB_SECRET_ACCESS_KEY");

            AmazonDynamoDBClient client;

            // create the config item to specify region
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.USEast1;

            client = new AmazonDynamoDBClient(accessKey, secretKey, clientConfig);

            return client;
        }

        [HttpPost]
        public static async Task<bool> AddReview(string bookID, string user, int rating, string review)
        {
            // checks that user has not already reviewed the book that day
            bool result = await ValidReview(bookID, user);
            if (!result)
            {
                return false;
            }

            string tableName = "BR_Reviews";

            DateTime date = DateTime.Today;
            string dateStr = date.Year.ToString() + date.Month.ToString("D2") + date.Day.ToString("D2");

            var item = new Dictionary<string, AttributeValue>
            {
                ["book"] = new AttributeValue { S = bookID },
                ["reviewID"] = new AttributeValue { S = user + dateStr },
                ["user"] = new AttributeValue { S = user },
                ["date"] = new AttributeValue { S = dateStr },
                ["rating"] = new AttributeValue { N = rating.ToString() },
                ["review"] = new AttributeValue { S = review },
                ["likecount"] = new AttributeValue { N = "0" },
            };

            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = item,
            };

            AmazonDynamoDBClient dbClient = dynamoDBConnect();
            var response = await dbClient.PutItemAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }

    
}
