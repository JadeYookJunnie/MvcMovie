using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon;
using System.Globalization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MvcMovie.Models
{
    public class BookModel
    {   
        public static AmazonDynamoDBClient dbClient;
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public List<string> Genre { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public DateTime PublishDate { get; set; }
        public int ReviewCount { get; set; }
        public List<ReviewModel> BookReviews { get; set; }


        public BookModel(string ISBN, string Title, string Description, string Picture, List<string> Genre, string Author, int Rating, DateTime PublishDate)
        {
            this.ISBN = ISBN;
            this.Title = Title;
            this.Description = Description;
            this.Picture = Picture;
            this.Genre = Genre;
            this.Author = Author;
            this.Rating = Rating;
            this.ReviewCount = 0;
            this.BookReviews = new List<ReviewModel>();
            this.PublishDate = PublishDate;

            dbClient = dynamoDBConnect();
            GetReviews();
        }
        public void addReview(ReviewModel review)
        {  
            BookReviews.Add(review);
        }
        public void RemoveReview(ReviewModel review)
        {  
            BookReviews.Remove(review);
        }

        public AmazonDynamoDBClient dynamoDBConnect(){
            string accessKey = Environment.GetEnvironmentVariable("DB_ACCESS_KEY");
            string secretKey = Environment.GetEnvironmentVariable("DB_SECRET_ACCESS_KEY");

            AmazonDynamoDBClient client;

            // create the config item to specify region
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.USEast1;

            client = new AmazonDynamoDBClient(accessKey, secretKey, clientConfig);

            return client;
        }

        public async Task<bool> GetReviews(){

            List<ReviewModel> reviews = new List<ReviewModel>();

            var request = new QueryRequest
            {
                TableName = "BR_Reviews",
                KeyConditionExpression = "book = :v_book",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_book", new AttributeValue { S =  ISBN }}}
            };

            var response = await dbClient.QueryAsync(request);

            // temp values to store response
            DateTime date = new DateTime();
            int rating = 0;
            string book = "";
            string reviewID = "";
            string review = "";
            int likeCount = 0;
            string user = "";

            string datePattern = "yyyyMMdd";

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                foreach (KeyValuePair<string, AttributeValue> pair in item){
                    if(pair.Key == "book"){
                        book = pair.Value.S;
                    }
                    else if(pair.Key == "reviewID"){
                        reviewID = pair.Value.S;
                    }
                    else if(pair.Key == "date"){
                        DateTime.TryParseExact(pair.Value.S, datePattern, null, DateTimeStyles.None, out date);
                    }
                    else if(pair.Key == "likecount"){
                        likeCount = int.Parse(pair.Value.N);
                    }
                    else if(pair.Key == "rating"){
                        rating = int.Parse(pair.Value.N);
                    }
                    else if(pair.Key == "review"){
                        review = pair.Value.S;
                    }
                    else if(pair.Key == "user"){
                        user = pair.Value.S;
                    }
                }
                
                // create review model from these values
                ReviewModel reviewModel = new ReviewModel(1, reviewID, null, this, likeCount, rating, review, date);
            } 

            BookReviews = reviews;
            return true;
        }
    }
}
