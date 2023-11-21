using Logic.DTO;
using MediatR;

namespace Logic.Queries.GetFilm;

public class GetFilmQuery : IRequest<FilmDto>
{
    public int FilmId { get; }

    public GetFilmQuery(int filmId)
    {
        FilmId = filmId;
    }
}