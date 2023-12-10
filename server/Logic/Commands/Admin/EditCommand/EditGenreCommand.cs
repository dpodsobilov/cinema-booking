using Data;
using Logic.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.EditCommand;

public class EditGenreCommand : IRequest
{
    public int GenreId { get; }
    public string GenreName { get; }
    public EditGenreCommand(AdminGenreDto adminGenreDto)
    {
        GenreId = adminGenreDto.GenreId;
        GenreName = adminGenreDto.GenreName;
    }
}

public class EditGenreCommandHandler : IRequestHandler<EditGenreCommand>
{
    private readonly ApplicationContext _applicationContext;

    public EditGenreCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(EditGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _applicationContext.Genres
            .Where(g => g.GenreId == request.GenreId)
            .FirstOrDefaultAsync(cancellationToken);

        if (genre == null)
        {
            throw new Exception("Такого жанра не существует!");
        }

        genre.GenreName = request.GenreName;
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}