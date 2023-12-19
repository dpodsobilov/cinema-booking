using Data;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin;

public class DeleteFilmCommand : IRequest
{
    public int FilmId { get; }

    public DeleteFilmCommand(int filmId)
    {
        FilmId = filmId;
    }
}

public class DeleteFilmCommandHandler : IRequestHandler<DeleteFilmCommand>
{
    private readonly ApplicationContext _applicationContext;

    public DeleteFilmCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(DeleteFilmCommand request, CancellationToken cancellationToken)
    {
        //получаем фильм
        var film = await _applicationContext.Films.Where(film => film.FilmId == request.FilmId 
                                                                 && film.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (film == null)
        {
            throw new NotFoundException("Выбранный фильм не существует");
        }
        
        //находим список непрошедших сеансов
        var sessions = await _applicationContext.Sessions
            .Where(session => session.FilmId == request.FilmId 
                   && session.IsDeleted == false && session.DataTimeSession > DateTime.Now)
            .ToListAsync(cancellationToken);
        
        if (sessions.Count != 0)
        {
            throw new NotAllowedException("На выбранный фильм есть сеансы!");
        }

        film.IsDeleted = true;
        
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}