using Data;
using Logic.DTO.Admin;
using Logic.Exceptions;
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
        // Проверяем, существует ли выбранный жанр
        var genre = await _applicationContext.Genres
            .Where(g => g.GenreId == request.GenreId)
            .FirstOrDefaultAsync(cancellationToken);

        if (genre == null)
        {
            throw new NotFoundException("Такого жанра не существует!");
        }
        
        // Проверяем, не используется ли жанр в фильмах (в таблице FilmGenre)
        var filmGenre = await _applicationContext.FilmGenres
            .Where(fg => fg.GenreId == request.GenreId)
            .FirstOrDefaultAsync(cancellationToken);

        if (filmGenre != null)
        {
            throw new NotAllowedException("Выбранный жанр уже присвоен фильмам!");
        }
        
        // Проверяем, существует ли другой жанр с выбранным названием
        var otherGenre = await _applicationContext.Genres
            .Where(g => g.GenreId != request.GenreId)
            .Where(g => g.GenreName.ToLower().Equals(request.GenreName.ToLower()))
            .FirstOrDefaultAsync(cancellationToken);

        if (otherGenre != null)
        {
            throw new NotAllowedException("Жанр с выбранным названием уже существует!");
        }

        genre.GenreName = request.GenreName;
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}