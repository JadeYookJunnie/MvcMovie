using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon;

namespace MvcMovie.Models
{
    public class MyUserModel : UserModel
    {
        public static AmazonDynamoDBClient dbClient;
        public string Name {get; set;}
        public string Password { get; set; }
        public List<BookModel> Favourites { get; set; } = new List<BookModel>(); 
        public List<BookModel> Wishlist { get; set; } = new List<BookModel>(); 
        public List<UserModel> Friendlist { get; set; } = new List<UserModel>(); 

        // have made these string lists for now so that the data from db can be stored easily
        public static List<string> StrFriends;
        public static List<string> StrFavourites;
        public static List<string> StrCurrentReads;
        public static List<string> StrWishlist;

        public List<ReviewModel> ReviewList { get; set; } = new List<ReviewModel>();

       public MyUserModel(string name, string password)
        {
            dbClient = dynamoDBConnect();

            // populates the local lists with data from db
            GetData("abbie");

            Name = name;
            Password = password; 
            CurrentReads = new List<BookModel>(); 
            Favorites = new List<BookModel>();
            Wishlist = new List<BookModel>();

            StrFriends = new List<string>();
            StrWishlist = new List<string>();
            StrFavourites = new List<string>();
            StrCurrentReads = new List<string>();
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

        public void addFriend(UserModel user)
        {
            Friendlist.Add(user);
        }

        public void removeFriend(UserModel user)
        {
            Friendlist.Remove(user);
        }

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


        public static async Task<bool> GetData(string username)
        {
            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var request = new GetItemRequest
            {
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.GetItemAsync(request);

            if(response.Item == null){
                return false;
            }

            foreach(var attribute in response.Item){
                if(attribute.Key == "friends"){
                    StrFriends = attribute.Value.SS;
                }
                else if(attribute.Key == "favourites"){
                    StrFavourites = attribute.Value.SS;
                }
                else if(attribute.Key == "currentreads"){
                    StrCurrentReads = attribute.Value.SS;
                }
                else if(attribute.Key == "wishlist"){
                    StrWishlist = attribute.Value.SS;
                }
            }

            // Console.WriteLine("favourites:");
            // foreach(string item in StrFavourites){
            //     Console.WriteLine(item);
            // }
            
            return true;
        }

        public static async Task<bool> CreateUser(string username)
        {
            // checks that username is not taken 
            bool result = await UsernameTaken(username);
            if(result){
                return false;
            }

            string tableName = "BR_User";

            var item = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = item,
            };

            var response = await dbClient.PutItemAsync(request);
            Console.WriteLine(response.HttpStatusCode);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public static async Task<bool> UsernameTaken(string username)
        {
            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var request = new GetItemRequest
            {
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.GetItemAsync(request);

            if(response.Item == null){
                return false;
            }
            return true;
        }

        public static async Task<bool> AddBookToFavourites(string username, string bookID)
        {
            // assuming that it is impossible for the username to not exist

            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["favourites"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.ADD,
                    Value = new AttributeValue { SS = {bookID} },
                },
            };

            var request = new UpdateItemRequest
            {
                AttributeUpdates = updates,
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.UpdateItemAsync(request);
            Console.WriteLine(response);

            Console.WriteLine(response.HttpStatusCode);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public static async Task<bool> RemoveBookFromFavourites(string username, string bookID)
        {
            // assuming that it is impossible for the username to not exist
            // assuming that has already been checked that the bookID is in the local favourites list

            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["favourites"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.DELETE,
                    Value = new AttributeValue { SS = {bookID} },
                },
            };

            var request = new UpdateItemRequest
            {
                AttributeUpdates = updates,
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.UpdateItemAsync(request);
            Console.WriteLine(response);

            Console.WriteLine(response.HttpStatusCode);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
       
        public static async Task<bool> AddBookToWishlist(string username, string bookID)
        {
            // assuming that it is impossible for the username to not exist

            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["wishlist"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.ADD,
                    Value = new AttributeValue { SS = {bookID} },
                },
            };

            var request = new UpdateItemRequest
            {
                AttributeUpdates = updates,
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.UpdateItemAsync(request);
            Console.WriteLine(response);

            Console.WriteLine(response.HttpStatusCode);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public static async Task<bool> AddBookToCurrentReads(string username, string bookID)
        {
            // assuming that it is impossible for the username to not exist

            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["currentreads"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.ADD,
                    Value = new AttributeValue { SS = {bookID} },
                },
            };

            var request = new UpdateItemRequest
            {
                AttributeUpdates = updates,
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.UpdateItemAsync(request);
            Console.WriteLine(response);

            Console.WriteLine(response.HttpStatusCode);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public static async Task<bool> AddFriend(string username, string friend_username)
        {
            // note - this is implemented as one-way friendship (more like following)

            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["friends"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.ADD,
                    Value = new AttributeValue { SS = {friend_username} },
                },
            };

            var request = new UpdateItemRequest
            {
                AttributeUpdates = updates,
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.UpdateItemAsync(request);
            Console.WriteLine(response);

            Console.WriteLine(response.HttpStatusCode);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public static async Task<bool> RemoveBookFromWishlist(string username, string bookID)
        {
            // assuming that it is impossible for the username to not exist
            // assuming that has already been checked that the bookID is in the local favourites list

            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["wishlist"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.DELETE,
                    Value = new AttributeValue { SS = {bookID} },
                },
            };

            var request = new UpdateItemRequest
            {
                AttributeUpdates = updates,
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.UpdateItemAsync(request);
            Console.WriteLine(response);

            Console.WriteLine(response.HttpStatusCode);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public static async Task<bool> RemoveBookFromCurrentReads(string username, string bookID)
        {
            // assuming that it is impossible for the username to not exist
            // assuming that has already been checked that the bookID is in the local favourites list

            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["favourites"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.DELETE,
                    Value = new AttributeValue { SS = {bookID} },
                },
            };

            var request = new UpdateItemRequest
            {
                AttributeUpdates = updates,
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.UpdateItemAsync(request);
            Console.WriteLine(response);

            Console.WriteLine(response.HttpStatusCode);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
       
        public static async Task<bool> RemoveFriend(string username, string friend_username)
        {
            // assuming that it is impossible for the username to not exist
            // assuming that users are already friends
            // note - friendship is one way

            string tableName = "BR_User";

            var key = new Dictionary<string, AttributeValue>
            {
                ["username"] = new AttributeValue { S = username },
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["friends"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.DELETE,
                    Value = new AttributeValue { SS = {friend_username} },
                },
            };

            var request = new UpdateItemRequest
            {
                AttributeUpdates = updates,
                Key = key,
                TableName = tableName,
            };

            var response = await dbClient.UpdateItemAsync(request);
            Console.WriteLine(response);

            Console.WriteLine(response.HttpStatusCode);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

    }
}
