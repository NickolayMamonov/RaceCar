using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Domain.Aggregates;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Features;

public record SimulateRaceCommand(Guid RaceId) : IRequest<Race>;

public class SimulateRaceCommandHandler : IRequestHandler<SimulateRaceCommand, Race>
{
    private readonly RaceContext _db;

    public SimulateRaceCommandHandler(RaceContext db)
    {
        _db = db;
    }

    public async Task<Race> Handle(SimulateRaceCommand request, CancellationToken cancellationToken)
    {
        // Исправленная строка: получение гонки по RaceId
        var test = await _db.Races.FirstAsync(r => r.Id == request.RaceId, cancellationToken);
        var race = await _db.Races
            .Include(r => r.Drivers)
            .FirstOrDefaultAsync(r => r.Id == request.RaceId, cancellationToken);
        if (race == null)
        {
            throw new Exception("Race not found");
        }

        var drivers = race.Drivers.ToList();

        if (drivers.Count == 0)
        {
            throw new Exception("No drivers in the race");
        }

        Random random = new Random();
        var winner = drivers[random.Next(drivers.Count)];

        // Установите победителя
        race.SetWinner(winner.Name.Value);

        _db.Races.Update(race);
        await _db.SaveChangesAsync(cancellationToken);

        return race;
    }
}