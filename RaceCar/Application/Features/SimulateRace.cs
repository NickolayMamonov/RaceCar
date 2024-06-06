using MediatR;
using Microsoft.EntityFrameworkCore;
using RaceCar.Domain.Aggregates;
using RaceCar.Infrastructure.Data;

namespace RaceCar.Application.Features;

public class SimulateRace
{
    public record SimulateRaceCommand(string Label) : IRequest<Race>;

    public class SimulateRaceCommandHandler : IRequestHandler<SimulateRaceCommand, Race>
    {
        private readonly RaceContext _db;

        public SimulateRaceCommandHandler(RaceContext db)
        {
            _db = db;
        }
       

        public async Task<Race> Handle(SimulateRaceCommand request, CancellationToken cancellationToken)
        {
            var race = await _db.Races.Include(r => r.DriverIds).FirstOrDefaultAsync(r => r.Label.Value == request.Label);            if (race == null)
            {
                throw new Exception("Race not found");
            }
            var drivers = race.DriverIds.ToList();
            
            Random random = new Random();
            var winner = drivers[random.Next(drivers.Count)];

            race.SetWinner(winner);

            _db.Races.Update(race);
            await _db.SaveChangesAsync();

            return race;
        }
    }
}