using Microsoft.EntityFrameworkCore;

namespace AbcRest_Final.Database_Context
{
    // Defines the database context class which is a part of Entity Framework
    public class ApplicationDbContext : DbContext
    {
        // Constructor accepts DbContextOptions and passes them to the base DbContext class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        // DbSet property representing the Users table in the database
        // Each DbSet corresponds to a table in the database, and each entity corresponds to a row in the table
        public DbSet<User> Users { get; set; }
    }
}
