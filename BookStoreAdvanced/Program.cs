using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole(); // Use the Console logger
});
builder.Services.AddEndpointsApiExplorer();
//Tunning SwagegrGen more verstion Flexible
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Book Store Manager",
            Description = "Book store selling Books and Merchandises",
            Version = "v3",
        });
    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
    c.IncludeXmlComments(filePath);
});
//Inject the mongoClient connection string
builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
{
    var uri = s.GetRequiredService<IConfiguration>()["MongoUri"];
    return new MongoClient(uri);
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Use SwaggerUI for showing summary
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Demo Api");
    c.RoutePrefix = "";
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
