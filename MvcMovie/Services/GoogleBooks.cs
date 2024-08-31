using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MvcMovie.Models;

namespace MvcMovie.Services
{
    public class GoogleBooksService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GoogleBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("BOOKS_API");
        }

  public async Task<List<BookModel>> SearchAllBooksAsync(string query)
{
    if (string.IsNullOrWhiteSpace(query))
    {
        query= "*";
    }

    var encodedQuery = Uri.EscapeDataString(query);
    var allBooks = new List<BookModel>();
    int startIndex = 0;
    const int maxResults = 40; // Google Books API max results per page

    while (true)
    {
        var url = $"https://www.googleapis.com/books/v1/volumes?q={encodedQuery}&key={_apiKey}&startIndex={startIndex}&maxResults={maxResults}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"API request failed with status code {response.StatusCode}. Response: {errorMessage}");
        }

        var booksResponse = await response.Content.ReadFromJsonAsync<GoogleBooksResponse>();
        var books = booksResponse?.Items?.Select(item => new BookModel(
            item.VolumeInfo?.IndustryIdentifiers?.FirstOrDefault()?.Identifier ?? "No ISBN",
            item.VolumeInfo?.Title ?? "No Title",
            item.VolumeInfo?.Description ?? "No Description",
            item.VolumeInfo?.ImageLinks?.Thumbnail ?? "No Image",
            item.VolumeInfo?.Categories ?? new List<string>(),
            item.VolumeInfo?.Authors?.FirstOrDefault() ?? "Unknown",
            (int)(item.VolumeInfo?.AverageRating ?? 0) // Casting float? to int
        )).ToList();

        if (books == null || books.Count == 0)
        {
            break; 
        }

        allBooks.AddRange(books);

        startIndex += maxResults; 
    }

    return allBooks;
}
    }

        public class GoogleBooksResponse
    {
        public List<GoogleBookItem> Items { get; set; }
    }

    public class GoogleBookItem
    {
        public VolumeInfo VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public List<IndustryIdentifier> IndustryIdentifiers { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ImageLinks ImageLinks { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Authors { get; set; }
        public float? AverageRating { get; set; } // Nullable float to handle missing values
    }

    public class IndustryIdentifier
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
    }

    public class ImageLinks
    {
        public string Thumbnail { get; set; }
    }

}
