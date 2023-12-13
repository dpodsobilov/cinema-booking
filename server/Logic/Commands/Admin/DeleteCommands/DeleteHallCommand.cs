using Data;
using Logic.Exceptions;
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
        //находим кинозал
        var hall = await _applicationContext.CinemaHalls
            .Where(hall => hall.CinemaHallId == request.CinemaHallId)
            .FirstOrDefaultAsync(cancellationToken);

        if (hall == null)
        {
            throw new NotFoundException("Выбранный кинозал не существует!");
        }
        
        //находим сеансы в кинозале
        var session = await _applicationContext.Sessions
            .Where(session => session.CinemaHallId == hall.CinemaHallId && session.IsDeleted == false 
                   && session.DataTimeSession > DateTime.Now)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (session != null)
        {
            throw new NotAllowedException("В выбранном кинозале есть сеансы");
        }
        
        hall.IsDeleted = true;
        
        var sessionsDelete = await _applicationContext.Sessions
            .Where(ses => ses.CinemaHallId == hall.CinemaHallId && ses.IsDeleted == false)
            .Select(ses => ses)
            .ToListAsync(cancellationToken);

        if (sessionsDelete.Count != 0)
        {
            foreach (var sd in sessionsDelete)
            {
                sd.IsDeleted = true;
            }
        }
        
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}