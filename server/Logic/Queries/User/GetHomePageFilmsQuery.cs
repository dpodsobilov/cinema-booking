using Data;
using Logic.DTO;
using Logic.DTO.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.User;

public class GetHomePageFilmsQuery : IRequest<IList<HomePageDto>>
{
    
}

public class GetHomePageFilmsQueryHandler : IRequestHandler<GetHomePageFilmsQuery, IList<HomePageDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetHomePageFilmsQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<HomePageDto>> Handle(GetHomePageFilmsQuery request, CancellationToken cancellationToken)
    {
        var cinemas = await _applicationContext.Sessions.Where(session => session.IsDeleted == false
                                            && session.DataTimeSession > DateTime.Now                              
                                            && session.CinemaHall.IsDeleted == false
                                            && session.CinemaHall.Cinema.IsDeleted == false
                                            && session.CinemaHall.CinemaHallType.IsDeleted == false)
            .Select(session => session.CinemaHall.Cinema)
            .Distinct().ToListAsync(cancellationToken);
        var homePageDto = new List<HomePageDto>();
        
        var noSessionFilm = await _applicationContext.Films
            .Join(_applicationContext.Sessions,
                film => film.FilmId,
                session => session.FilmId,
                (film, session) => new { FilmId = film.FilmId, SessionDeleted = session.IsDeleted })
            .GroupBy(x => x.FilmId)
            .Where(group => group.All(x => x.SessionDeleted == true))
            .Select(group => group.Key)
            .ToListAsync(cancellationToken);
        
        foreach (var cinema in cinemas)
        {
            var films = await _applicationContext.Sessions.
                Where(session => session.CinemaHall.Cinema.CinemaId == cinema.CinemaId 
                                 && session.DataTimeSession > DateTime.Now
                                 && session.Film.IsDeleted == false
                                 && session.CinemaHall.IsDeleted == false
                                 && session.CinemaHall.Cinema.IsDeleted == false
                                 && session.CinemaHall.CinemaHallType.IsDeleted == false
                                 && !noSessionFilm.Contains(session.FilmId))
                .Where(film => _applicationContext.Sessions
                    .Any(session => session.FilmId == film.FilmId && !session.IsDeleted))
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