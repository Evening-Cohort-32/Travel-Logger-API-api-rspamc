using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelLogger.Models;

namespace TravelLogger;

public class TravelLoggerDbContext : DbContext
{

    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    
    public TravelLoggerDbContext(DbContextOptions<TravelLoggerDbContext> context) : base(context)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // seed your data here
          modelBuilder.Entity<City>().HasData(new City[]
    {
            new City {Id = 1, Name = "Trent"},
            new City {Id = 2, Name = "Clearwater"},
            new City {Id = 3, Name = "Chicago"}
    });
    }
}