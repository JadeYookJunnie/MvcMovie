using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Models
{
    public class UserModel
    {
        public String UserId { get; set; }
        public string Name { get; set; }
        public List<BookModel> Favorites { get; set; }
        public List<BookModel> Wishlist { get; set; }
        public List<BookModel> CurrentReads { get; set; }
        public List<UserModel> Friends { get; set; }
        public List<ReviewModel> MyReviews { get; set; }


        private readonly IDynamoDBContext _context;

        private readonly IAmazonDynamoDB _dynamoDB;

        public List<BookModel> getFavorites()
        {
            return new List<BookModel>();
        }

        public List<BookModel> getWishlist()
        {
            return new List<BookModel>();
        }

        public List<UserModel> getFriends()
        {
            return new List<UserModel>();
        }

        public List<ReviewModel> getReviewModels() { 
            return new List<ReviewModel>();
        }

        public List<BookModel> getCurrentReads() {
            return new List<BookModel>();
        }

        public async Task<bool> CreateUser(string _tableName)
        {
            var userAsJson = JsonSerializer.Serialize(this);
            var userAsAttributes = Document.FromJson(userAsJson).ToAttributeMap();

            var createItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = userAsAttributes
            };

            var response = await _dynamoDB.PutItemAsync(createItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        } 

        public async Task<UserModel?> GetUserAsync(string _tableName)
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "user_id", new AttributeValue { S = UserId } }
            }
            };

            try
            {
                var response = await _dynamoDB.GetItemAsync(request);
                if (response.Item == null || response.Item.Count == 0)
                {
                    Console.WriteLine("User not found.");
                    return null;
                }

                var itemAsDocument = Document.FromAttributeMap(response.Item);

                return JsonSerializer.Deserialize<UserModel>(itemAsDocument.ToJson());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateBookAsync(string _tableName)
        {
            //this.UpdatedAt = DateTime.UtcNow;
            var userAsJson = JsonSerializer.Serialize(this);
            var userAsAttributes = Document.FromJson(userAsJson).ToAttributeMap();

            var updateItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = userAsAttributes
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
                { "pk", new AttributeValue { S = UserId.ToString() } },
                { "sk", new AttributeValue { S = UserId.ToString() } }
            }
            };

            var response = await _dynamoDB.DeleteItemAsync(deleteItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

    }
}