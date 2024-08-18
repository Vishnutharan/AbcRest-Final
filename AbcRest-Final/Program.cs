using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AbcRest_Final.Database_Context;  // Includes the namespace where ApplicationDbContext is defined
using Microsoft.EntityFrameworkCore;  // Includes the EF Core namespace for database operations

var builder = WebApplication.CreateBuilder(args);  // Initializes a builder for the web application with command line arguments

// Add services to the container.
builder.Services.AddControllers();  // Adds MVC controllers to the application's services

// Configure Entity Framework and the database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  // Configures the application to use a SQL Server database with connection string from app settings

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")  // Specify the client application origin
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();  // Adds API exploration services to support Swagger/OpenAPI
builder.Services.AddSwaggerGen();  // Adds Swagger generator services to produce Swagger documentation for the API

var app = builder.Build();  // Builds the web application ready to start accepting requests

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Enables Swagger when the application is in development environment
    app.UseSwaggerUI();  // Enables Swagger UI in development environment
}

app.UseHttpsRedirection();  // Redirects HTTP requests to HTTPS

app.UseCors("AllowSpecificOrigin");  // Apply the CORS policy globally

app.UseAuthorization();  // Adds Authorization middleware to the request pipeline to secure the app

app.MapControllers();  // Maps attribute-routed controllers to the app

app.Run();  // Runs the application
