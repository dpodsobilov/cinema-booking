using Data;
using Logic.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.GetHomePageFilms;

public class GetHomePageFilmsQueryHandler : IRequestHandler<GetHomePageFilmsQuery, IList<HomePageDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetHomePageFilmsQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<HomePageDto>> Handle(GetHomePageFilmsQuery request, CancellationToken cancellationToken)
    {
        // var films = await _applicationContext.Films.Select(film => new HomePageFilmDTO
        // {
        //     FilmId = film.FilmId,
        //     FilmName = film.FilmName,
        //     Poster = film.Poster
        // }).ToListAsync(cancellationToken);
        var cinemas = await _applicationContext.Sessions.Select(session => session.CinemaHall.Cinema)
            .Distinct().ToListAsync(cancellationToken);
        var homePageDto = new List<HomePageDto>();
        foreach (var cinema in cinemas)
        {
            var films = await _applicationContext.Sessions.Where(session => session.CinemaHall.Cinema.CinemaId == cinema.CinemaId)
                .Select(session => new HomePageFilmDto
                {
                    FilmId = session.FilmId,
                    FilmName = session.Film.FilmName,
                    Poster = session.Film.Poster
                }).Distinct().ToListAsync(cancellationToken);
            homePageDto.Add(new HomePageDto
            {
                CinemaId = cinema.CinemaId,
                CinemaName = cinema.CinemaName,
                Films = films
            });
        }
        return homePageDto;
    }
}