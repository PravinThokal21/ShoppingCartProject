using Microsoft.EntityFrameworkCore;
using ShoppingCart.API;
using ShoppingCart.API.Models;
using ShoppingCart.API.Services.Implementations;
using ShoppingCart.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Add services

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ShoppingCartContext>(opt =>
    opt.UseInMemoryDatabase("ShoppingCart"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IProductService, ProductService>().AddHttpClient<ProductService>();
builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();

// Add middleware
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();

//app.MapGet("/", () => "Hello World!");

app.Run();
