using Data;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin;

public class DeleteGenreCommand : IRequest
{
    public int GenreId { get; }

    public DeleteGenreCommand(int genreId)
    {
        GenreId = genreId;
    }
}

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand>
{
    private readonly ApplicationContext _applicationContext;

    public DeleteGenreCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _applicationContext.Genres.Where(genre => genre.GenreId == request.GenreId 
                                                                    && genre.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        if (genre != null)
        {
            genre.IsDeleted = true;
            await _applicationContext.SaveChangesAsync(cancellationToken);
        }
        else throw new NotFoundException("Выбранный жанр не существует!"); 
    }
}