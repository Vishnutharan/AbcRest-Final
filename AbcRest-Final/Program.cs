using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using AbcRest_Final.Database_Context;
using AbcRest_Final.Interface;
using AbcRest_Final.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Configure Entity Framework and the database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the generic repository for all entities
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register the BookingService and EmailService
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<EmailService>();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Add Swagger to the application for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Optionally add Health Checks (if needed)
builder.Services.AddHealthChecks();

// Optionally add Memory Caching (if needed)
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Use developer exception page to see detailed errors
    app.UseSwagger();                 // Enable Swagger for API documentation
    app.UseSwaggerUI();               // Enable Swagger UI for easy testing
}

// Automatically apply database migrations and seed data (if needed)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();  // Automatically apply migrations
    // Seed data if necessary
    // SeedData.Initialize(dbContext); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin"); // Apply CORS policy globally

// Optionally use health checks
app.UseHealthChecks("/health");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();   // Maps attribute-routed controllers to the app
    endpoints.MapRazorPages();    // Maps Razor Pages endpoints
    // Optionally map health checks endpoint
    endpoints.MapHealthChecks("/health");
});

app.Run();  // Runs the application
