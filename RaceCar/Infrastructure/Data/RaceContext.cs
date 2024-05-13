using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
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
        modelBuilder.Entity<RaceDto>(race =>
        {
            race.ToTable("Races"); // Указываем имя таблицы
            race.HasKey(r => r.Id); // Указываем первичный ключ
            race.Property(d => d.Label).IsRequired(); // Устанавливаем ограничения для свойства Label
            race.HasMany(r => r.DriverIds); // Отношение "один ко многим" с водителями
        });

        modelBuilder.Entity<Driver>(driver =>
        {
            driver.ToTable("Drivers"); // Указываем имя таблицы
            driver.HasKey(d => d.Id); // Указываем первичный ключ
            driver.Property(d => d.Name).IsRequired(); // Устанавливаем ограничения для свойства Name
            driver.Property(d => d.CarType).IsRequired(); // Устанавливаем ограничения для свойства CarType
            driver.Property(d => d.HorsePower).IsRequired(); // Устанавливаем ограничения для свойства HorsePower
            driver.HasOne(d => d.RaceId); // Отношение "один к одному" с гонкой
        });
    }
}
