using Data;
using Logic.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries;

public class GetFilmQuery : IRequest<FilmDto>
{
    public int FilmId { get; }

    public GetFilmQuery(int filmId)
    {
        FilmId = filmId;
    }
}

public class GetFilmQueryHandler : IRequestHandler<GetFilmQuery, FilmDto>
{
    private readonly ApplicationContext _applicationContext;

    public GetFilmQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<FilmDto> Handle(GetFilmQuery request, CancellationToken cancellationToken)
    {
        var film = await _applicationContext.Films.Where(film => film.FilmId == request.FilmId)
            .Select(film => new FilmDto
            {
                FilmId = film.FilmId,
                FilmName = film.FilmName,
                Duration = film.Duration,
                Description = film.Description,
                Poster = film.Poster,
                FilmGenres = new List<string>()
            }).FirstOrDefaultAsync(cancellationToken);
        var genres = await _applicationContext.FilmGenres.Where(filmgenre => filmgenre.FilmId == request.FilmId)
            .Select(filmGenre => filmGenre.Genre.GenreName).ToListAsync(cancellationToken);

        film.FilmGenres = genres;
        return film;
    }
}