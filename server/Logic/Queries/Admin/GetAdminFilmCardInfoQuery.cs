using Data;
using Logic.DTO.Admin.ForEditing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace Logic.Queries.Admin;

public class GetAdminFilmCardInfoQuery : IRequest<EditFilmDto> {
    public int FilmId { get; }

    public GetAdminFilmCardInfoQuery(int FilmId)
    {
        this.FilmId = FilmId;
    }
}

public class GetAdminFilmCardInfoQueryHandler : IRequestHandler<GetAdminFilmCardInfoQuery, EditFilmDto>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminFilmCardInfoQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<EditFilmDto> Handle(GetAdminFilmCardInfoQuery request, CancellationToken cancellationToken)
    {
        var film = await _applicationContext.Films.Where(film => film.FilmId == request.FilmId).FirstOrDefaultAsync(cancellationToken);

        if (film == null)
        {
            throw new Exception("Выбранного фильма не существует!");
        }

        var filmGenres = await _applicationContext.FilmGenres.Where(filmGenres => filmGenres.FilmId == film.FilmId)
            .Select(fg => fg.GenreId).ToArrayAsync(cancellationToken);

        var filmCardInfoDto = new EditFilmDto
        {
            FilmId = film.FilmId,
            FilmName = film.FilmName,
            Description = film.Description,
            FilmCoefficient = film.FilmCoefficient,
            Year = film.Year,
            Hours = film.Duration.Split(" ")[0],
            Minutes = film.Duration.Split(" ")[2],
            Poster = film.Poster,
            Genres = filmGenres
        };

        return filmCardInfoDto;
    }
}