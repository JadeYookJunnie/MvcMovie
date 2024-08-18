using Blazorise;
using Blazorise.AntDesign;
using Blazorise.Icons.FontAwesome;
using DotNetEnv; 
using MvcMovie.Services;
var builder = WebApplication.CreateBuilder(args);

// Load environment variables
Env.Load();
string googleBooksApiKey = Environment.GetEnvironmentVariable("BOOKS_API");

// Register services
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<GoogleBooksService>(client =>
{
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {googleBooksApiKey}");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
