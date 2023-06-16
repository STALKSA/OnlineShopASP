
using Microsoft.AspNetCore.Http.Json;
using OnlineShopASP;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICatalog, InMemoryCatalog>(); // Регистрация зависимости

builder.Services.Configure<JsonOptions>(
   options =>
   {
       options.SerializerOptions.WriteIndented = true;
   }
);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

InMemoryCatalog catalog = new InMemoryCatalog();

app.MapPost("/add_product", AddProduct);
app.MapGet("/get_products", GetProducts);
app.MapGet("/get_product", GetProductById);
app.MapPost("/update_product", UpdateProductById);
app.MapPost("/delete_product", DeleteProductById);
app.MapPost("/clear_products", ClearProducts);


void AddProduct(Product product,ICatalog catalog, HttpContext context)
{
    catalog.AddProduct(product);
    context.Response.Headers.Add("Location", $"/products/{product.Id}");
    context.Response.StatusCode = StatusCodes.Status201Created;
}

ConcurrentDictionary<Guid, Product> GetProducts(ICatalog catalog)
{
    return catalog.GetProducts();
}

Product GetProductById(string id, ICatalog catalog)
{
    return catalog.GetProductById(Guid.Parse(id));
}

void UpdateProductById(string productId, Product newProduct, ICatalog catalog, HttpContext context)
{
    catalog.UpdateProductById(Guid.Parse(productId), newProduct);
    context.Response.StatusCode = StatusCodes.Status200OK;
}

void DeleteProductById(string productId, ICatalog catalog, HttpContext context)
{
    catalog.DeleteProductById(Guid.Parse(productId));
    context.Response.StatusCode = StatusCodes.Status204NoContent;
}

void ClearProducts(ICatalog catalog, HttpContext context)
{
    catalog.ClearProducts();
    context.Response.StatusCode = StatusCodes.Status204NoContent;
}


app.Run();
