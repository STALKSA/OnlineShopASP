
using MailKit.Security;
using Microsoft.AspNetCore.Http.Json;
using MimeKit.Text;
using MimeKit;
using OnlineShopASP;
using System.Collections.Concurrent;
using MailKit.Net.Smtp;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

Log.Logger = new LoggerConfiguration()
   .WriteTo.Console()
   .CreateBootstrapLogger(); //означает, что глобальный логер будет заменен на вариант из Host.UseSerilog
Log.Information("Запуск");

try
{

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, conf) =>   //логирование в файл
{
    conf
        .WriteTo.Console()
        .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day);
});

builder.WebHost.UseSentry();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<SmtpConfig>()
    .BindConfiguration("SmtpConfig")
    .ValidateDataAnnotations()
    .ValidateOnStart();


builder.Services.AddSingleton<ICatalog, InMemoryCatalog>();                              // Регистрация зависимости каталог
//builder.Services.AddSingleton<IClock, UtcClock>();                                    // Регистрация зависимости Time
builder.Services.AddScoped<IEmailSender, MailKitSmtpEmailSender>();                    //Регистрация отправки почты
builder.Services.Decorate<IEmailSender, EmailSenderLoggingDecorator>();               //Декоратор логирования
builder.Services.Decorate<IEmailSender, EmailSenderRetryDecorator>();                //Декоратор повторной отправки сообщений
//builder.Services.AddHostedService<AppStartedNotificatorBackgroundService>();      //Регистрация фонового сервиса
//builder.Services.AddHostedService<SalesNotificatorBackgroundService>();     

   //builder.Services.Configure<JsonOptions>(
   //   options =>
   //   {
   //       options.SerializerOptions.WriteIndented = true;
   //   }
   //);


   var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSentryTracing();


    app.MapPost("/add_product", AddProduct);
    app.MapGet("/get_products", GetProducts);
    app.MapGet("/get_product", GetProductById);
    app.MapPost("/update_product", UpdateProductById);
    app.MapPost("/delete_product", DeleteProductById);
    app.MapPost("/clear_products", ClearProducts);

    app.MapGet("/send_email", SendEmail);

    async Task SendEmail(string recepientEmail, string subject, string body, IEmailSender emailSender)
    {
        await emailSender.SendEmail(recepientEmail, subject, body);
    }

    void AddProduct(Product product, ICatalog catalog, HttpContext context)
    {
        catalog.AddProduct(product);
        context.Response.Headers.Add("Location", $"/products/{product.Id}");
        context.Response.StatusCode = StatusCodes.Status201Created;
    }

    List<Product> GetProducts(ICatalog catalog)
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

}
catch (Exception ex)
{
    Log.Fatal(ex, "Сервер упал");
}
finally
{
    Log.Information("Завершение работы");
    await Log.CloseAndFlushAsync(); //перед выходом дожидаемся пока все логи будут записаны
}


