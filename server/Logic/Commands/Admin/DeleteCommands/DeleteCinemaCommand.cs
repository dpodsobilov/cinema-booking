using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin;

public class DeleteCinemaCommand : IRequest
{
    public int CinemaId { get; }

    public DeleteCinemaCommand(int cinemaId)
    {
        CinemaId = cinemaId;
    }
}

public class DeleteCinemaCommandHandler : IRequestHandler<DeleteCinemaCommand>
{
    private readonly ApplicationContext _applicationContext;

    public DeleteCinemaCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(DeleteCinemaCommand request, CancellationToken cancellationToken)
    {
        var hallId = new List<int>();
        
        var cinema = await _applicationContext.Cinemas
            .Where(cinema => cinema.CinemaId == request.CinemaId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (cinema != null)
        {
            cinema.IsDeleted = true;
            
            var halls = await _applicationContext.CinemaHalls
                .Where(hall => hall.CinemaId == request.CinemaId)
                .Select(hall => hall)
                .ToListAsync(cancellationToken);

            if (halls.Count != 0)
            {
                foreach (var hall in halls)
                {
                    hall.IsDeleted = true;

                    hallId.Add(hall.CinemaHallId);
                }

                var sessions = await _applicationContext.Sessions
                    .Where(session => hallId.Contains(session.CinemaHallId))
                    .Select(session => session)
                    .ToListAsync(cancellationToken);

                if (sessions.Count != 0)
                {
                    foreach (var session in sessions)
                    {
                        session.IsDeleted = true;
                    }
                }
                else throw new Exception("Ошибка!");
            }
            else throw new Exception("Ошибка!");
            
            await _applicationContext.SaveChangesAsync(cancellationToken);
        }
        else throw new Exception("Ошибка!");
    }
}