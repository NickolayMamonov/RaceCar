using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Application.DTO;
using RaceCar.Domain.Aggregates;
using RaceCar.Domain.Entities;
using RaceCar.Domain.ValueObjects;

namespace RaceCar.Infrastructure.Data;

public class RaceContext : DbContext
{
    public DbSet<Race> Races { get; set; }
    public DbSet<Driver> Drivers { get; set; }

    private IMediator _mediator;


    public RaceContext(DbContextOptions<RaceContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public RaceContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Driver>().ToTable(nameof(Driver));

        modelBuilder.Entity<Driver>().HasKey(r => r.Id);
        modelBuilder.Entity<Driver>().Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(driverId => driverId.Value, dbId => DriverId.Of(dbId));

        modelBuilder.Entity<Driver>().ComplexProperty(p => p.Name, p =>
        {
            p.Property(x => x.Value).HasColumnName(nameof(Driver.Name))
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Driver>().ComplexProperty(p => p.CarType, p =>
        {
            p.Property(x => x.Value).HasColumnName(nameof(Driver.CarType))
                .IsRequired()
                .HasMaxLength(10);
        });

        modelBuilder.Entity<Driver>().ComplexProperty(p => p.HorsePower, p =>
        {
            p.Property(x => x.Value).HasColumnName(nameof(Driver.HorsePower))
                .IsRequired()
                .HasMaxLength(10000);
        });


        modelBuilder.Entity<Race>().ToTable(nameof(Race));

        modelBuilder.Entity<Race>().HasKey(r => r.Id);
        modelBuilder.Entity<Race>().Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(raceId => raceId.Value, dbId => RaceId.Of(dbId));

        modelBuilder.Entity<Race>().ComplexProperty(p => p.Label, p =>
        {
            p.Property(x => x.Value).HasColumnName(nameof(Race.Label))
                .IsRequired()
                .HasMaxLength(50);
        });
        modelBuilder.Entity<Race>().ComplexProperty(p => p.TypeOfCar, p =>
        {
            p.Property(x => x.Value).HasColumnName(nameof(Race.TypeOfCar))
                .IsRequired()
                .HasMaxLength(10);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        // Dispatch Domain Events collection.
        await DispatchEvents(cancellationToken);

        return result;
    }

    private async Task DispatchEvents(CancellationToken cancellationToken)
    {
        var domainEntities = ChangeTracker
            .Entries<IAggregate>()
            .Where(x => x.Entity.GetDomainEvents() != null && x.Entity.GetDomainEvents().Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.GetDomainEvents())
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);
    }


    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //     
    //     // modelBuilder.Entity<RaceDto>(race =>
    //     // {
    //     //     race.ToTable("Races"); // Указываем имя таблицы
    //     //     race.HasKey(r => r.Id); // Указываем первичный ключ
    //     //     race.Property(d => d.Label).IsRequired(); // Устанавливаем ограничения для свойства Label
    //     //     race.HasMany(r => r.DriverIds); // Отношение "один ко многим" с водителями
    //     // });
    //     //
    //     // modelBuilder.Entity<Driver>(driver =>
    //     // {
    //     //     driver.ToTable("Drivers"); // Указываем имя таблицы
    //     //     driver.HasKey(d => d.Id); // Указываем первичный ключ
    //     //     driver.Property(d => d.Name).IsRequired(); // Устанавливаем ограничения для свойства Name
    //     //     driver.Property(d => d.CarType).IsRequired(); // Устанавливаем ограничения для свойства CarType
    //     //     driver.Property(d => d.HorsePower).IsRequired(); // Устанавливаем ограничения для свойства HorsePower
    //     //     driver.Property(d => d.RaceId).IsRequired(false); // Устанавливаем ограничения для свойства RaceId
    //     // });
    // }
}