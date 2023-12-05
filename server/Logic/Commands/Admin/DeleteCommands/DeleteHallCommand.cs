using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin;

public class DeleteHallCommand : IRequest
{
    public int CinemaHallId { get; }

    public DeleteHallCommand(int cinemaHallId)
    {
        CinemaHallId = cinemaHallId;
    }
}

public class DeleteHallCommandHandler : IRequestHandler<DeleteHallCommand>
{
    private readonly ApplicationContext _applicationContext;

    public DeleteHallCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(DeleteHallCommand request, CancellationToken cancellationToken)
    {
        var hallId = new List<int>();
        
        var halls = await _applicationContext.CinemaHalls
            .Where(hall => hall.CinemaHallId == request.CinemaHallId)
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
}