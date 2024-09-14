using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Net;
using System.Text.Json;

namespace MvcMovie.Models
{
    public class BookModel
    {   
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


<<<<<<< HEAD
        private readonly IDynamoDBContext _context;

        private readonly IAmazonDynamoDB _dynamoDB;

        public BookModel(string ISBN, string Title, string Description, string Picture, List<string> Genre, string Author, int Rating, int ReviewCount){
=======
        public BookModel(string ISBN, string Title, string Description, string Picture, List<string> Genre, string Author, int Rating, DateTime PublishDate)
        {
>>>>>>> origin/FrontendRefinements
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
        }
<<<<<<< HEAD


        public void setFavorite()
        {

        }

        public void addWishList() { }
        
        public void addReview() { }

        public async Task<bool> CreateBook(string _tableName)
        {
            var bookAsJson = JsonSerializer.Serialize(this);
            var bookAsAttributes = Document.FromJson(bookAsJson).ToAttributeMap();

            var createItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = bookAsAttributes
            };

            var response = await _dynamoDB.PutItemAsync(createItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<BookModel?> GetBookAsync(string _tableName)
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "book_id", new AttributeValue { S = ISBN } }
            }
            };

            try
            {
                var response = await _dynamoDB.GetItemAsync(request);
                if (response.Item == null || response.Item.Count == 0)
                {
                    Console.WriteLine("Book not found.");
                    return null;
                }

                var itemAsDocument = Document.FromAttributeMap(response.Item);

                return JsonSerializer.Deserialize<BookModel>(itemAsDocument.ToJson());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching book: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateBookAsync(string _tableName)
        {
            //this.UpdatedAt = DateTime.UtcNow;
            var bookAsJson = JsonSerializer.Serialize(this);
            var bookAsAttributes = Document.FromJson(bookAsJson).ToAttributeMap();

            var updateItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = bookAsAttributes
            };

            var response = await _dynamoDB.PutItemAsync(updateItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> DeleteAsync(string _tableName)
        {
            var deleteItemRequest = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = ISBN.ToString() } },
                { "sk", new AttributeValue { S = ISBN.ToString() } }
            }
            };

            var response = await _dynamoDB.DeleteItemAsync(deleteItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
=======
        public void addReview(ReviewModel review)
        {  
            BookReviews.Add(review);
        }
        public void RemoveReview(ReviewModel review)
        {  
            BookReviews.Remove(review);
        }
>>>>>>> origin/FrontendRefinements
    }
}
