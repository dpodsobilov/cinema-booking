using Data;
using Data.Models;
using Logic.Exceptions;
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
        //находим кинотеатр
        var cinema = await _applicationContext.Cinemas
            .Where(cinema => cinema.CinemaId == request.CinemaId && cinema.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (cinema == null)
        {
            throw new NotFoundException("Выбранный кинотеатр не существует!");
        }
        
        //находим список кинозалов у кинотеатра
        var halls = await _applicationContext.CinemaHalls
            .Where(hall => hall.CinemaId == request.CinemaId && hall.IsDeleted == false)
            .Select(hall => hall)
            .ToListAsync(cancellationToken);

        if (halls.Count == 0)
        {
            cinema.IsDeleted = true;
            await _applicationContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            int counter = 0;
            foreach (var hall in halls)
            {
                var sessions = await _applicationContext.Sessions
                    .Where(session => session.CinemaHallId == hall.CinemaHallId && session.IsDeleted == false
                    && session.DataTimeSession > DateTime.Now)
                    .Select(session => session)
                    .ToListAsync(cancellationToken);

                if (sessions.Count == 0) { counter++; }
            }

            if (counter == halls.Count)
            {
                cinema.IsDeleted = true;
                foreach (var hall in halls)
                {
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
                }
                await _applicationContext.SaveChangesAsync(cancellationToken);
            }
            else throw new NotAllowedException("В выбранном кинотеатре есть сеансы!");
        }
    }
}