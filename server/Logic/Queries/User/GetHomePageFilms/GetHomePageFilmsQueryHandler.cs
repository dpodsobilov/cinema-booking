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
        // ДОБАВИТЬ ПРОВЕРКИ ПО ДАТЕ У СЕАНСОВ
        
        var cinemas = await _applicationContext.Sessions.Where(session => session.IsDeleted == false
                                            && session.CinemaHall.IsDeleted == false
                                            && session.CinemaHall.Cinema.IsDeleted == false
                                            && session.CinemaHall.CinemaHallType.IsDeleted == false)
            .Select(session => session.CinemaHall.Cinema)
            .Distinct().ToListAsync(cancellationToken);
        var homePageDto = new List<HomePageDto>();
        foreach (var cinema in cinemas)
        {
            var films = await _applicationContext.Sessions.
                Where(session => session.CinemaHall.Cinema.CinemaId == cinema.CinemaId 
                                 && session.Film.IsDeleted == false
                                 && session.CinemaHall.IsDeleted == false
                                 && session.CinemaHall.Cinema.IsDeleted == false
                                 && session.CinemaHall.CinemaHallType.IsDeleted == false)
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