using Data;
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
        var film = await _applicationContext.Films.Where(film => film.FilmId == request.FilmId)
            .FirstOrDefaultAsync(cancellationToken);
        if (film != null)
        {
            _applicationContext.Films.Remove(film);
            await _applicationContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new Exception("Такого фильма нет!");
        }
    }
}