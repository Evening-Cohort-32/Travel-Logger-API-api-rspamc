using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelLogger.Models;

namespace TravelLogger;

public class TravelLoggerDbContext : DbContext
{

    public DbSet<User> Users { get; set; }
    
    public TravelLoggerDbContext(DbContextOptions<TravelLoggerDbContext> context) : base(context)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // seed your data here

    }
}