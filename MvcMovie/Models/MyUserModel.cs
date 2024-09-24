﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon;
using MvcMovie.Services;

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

        public List<ReviewModel> ReviewList { get; set; } = new List<ReviewModel>();

        // have made these string lists for now so that the data from db can be stored easily
        public static List<string> StrFriends;
        public static List<string> StrFavourites;
        public static List<string> StrCurrentReads;
        public static List<string> StrWishlist;
        public static List<string> StrBooksReviewed;

        public MyUserModel(string name)
        {
            Name = name;

            dbClient = dynamoDBConnect();

            // populates the local lists with data from db
            GetData(name);

            CurrentReads = new List<BookModel>(); 
            Favorites = new List<BookModel>();   
            Wishlist = new List<BookModel>();

            StrFriends = new List<string>();
            StrWishlist = new List<string>();
            StrFavourites = new List<string>();
            StrCurrentReads = new List<string>(); 
            StrBooksReviewed = new List<string>();
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

        // Turns the List<String> of book ISBNs to List<BookModel> to be displayed
        public async Task IdsToBookModel(GoogleBooksService booksService){

            StrCurrentReads = ["9780226062181", "9781922459541"];
            foreach (string bookId in StrCurrentReads){
                // https://stackoverflow.com/questions/7908954/google-books-api-searching-by-isbn
                // https://www.googleapis.com/books/v1/volumes?q=isbn:<your_isbn_here>
                string query = "isbn:" + bookId;
                var books = await booksService.SearchAllBooksAsync(query);

                CurrentReads.Add(books[0]);
                
            }

            // repeat the foreach for favourites

            // repeat the foreach for wishlist

            // note: not here but will probably have to turn StrReviews into the ReviewModel
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


    }
}
