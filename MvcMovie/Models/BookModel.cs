﻿using Amazon.DynamoDBv2;
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
        public string Picture { get; set; } //for imagesource, can change
        public List<string> Genre { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public int ReviewCount { get; set; }

        private readonly IDynamoDBContext _context;

        private readonly IAmazonDynamoDB _dynamoDB;

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
    }
}
