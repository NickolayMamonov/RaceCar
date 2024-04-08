using Microsoft.EntityFrameworkCore;
using RaceCar.Domain.Aggregates;

namespace RaceCar.Infrastructure.Data;
public class RaceContext : DbContext
{
    public DbSet<Race> Races { get; set; }
    public DbSet<Driver> Drivers { get; set; }

    public RaceContext(DbContextOptions<RaceContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Race>()
            .HasMany(r => r.Drivers)
            .WithOne()
            .HasForeignKey(d => d.RaceId);
    }
}
