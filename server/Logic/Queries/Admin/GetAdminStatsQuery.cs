using Data;
using Logic.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminStatsQuery : IRequest<IList<AdminStatDto>> { }

public class GetAdminStatsQueryHandler : IRequestHandler<GetAdminStatsQuery, IList<AdminStatDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminStatsQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IList<AdminStatDto>> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
    {

        var orderedTicketsFilms = await _applicationContext.Films
            .GroupJoin(
                _applicationContext.Sessions,
                film => film.FilmId,
                session => session.FilmId,
                (film, sessions) => new
                {
                    FilmName = film.FilmName,
                    TicketCount = sessions
                    .Join(
                        _applicationContext.Tickets,
                        session => session.SessionId,
                        ticket => ticket.SessionId,
                        (session, ticket) => ticket)
                    .Count()
                }
            ).ToListAsync(cancellationToken);

        var totalTicketsFilms = await _applicationContext.Films
                .GroupJoin(
                    _applicationContext.Sessions,
                    film => film.FilmId,
                    session => session.FilmId,
                    (film, sessions) => new
                    {
                        FilmName = film.FilmName,
                        PlaceCount = sessions
                            .Join(
                                _applicationContext.CinemaHalls,
                                session => session.CinemaHallId,
                                hall => hall.CinemaHallId,
                                (session, hall) => hall)
                            .Join(
                                _applicationContext.CinemaHallTypes,
                                hall => hall.CinemaHallTypeId,
                                type => type.CinemaHallTypeId,
                                (hall, type) => type)
                            .Join(
                                _applicationContext.Places,
                                type => type.CinemaHallTypeId,
                                place => place.CinemaHallTypeId,
                                (type, place) => place)
                            .Count()
                    })
                .ToListAsync(cancellationToken);

        totalTicketsFilms = totalTicketsFilms.OrderBy(f => f.FilmName).ToList();

        orderedTicketsFilms = orderedTicketsFilms.OrderBy(f => f.FilmName).ToList();


        var statDto = new List<AdminStatDto>();

        for (int i  = 0; i < orderedTicketsFilms.Count(); ++i)
        {
            statDto.Add(new AdminStatDto
            {
                FilmName = orderedTicketsFilms[i].FilmName,
                OrderedTickets = orderedTicketsFilms[i].TicketCount,
                TotalTickets = totalTicketsFilms[i].PlaceCount,
                Percentage = (int)(totalTicketsFilms[i].PlaceCount == 0 
                            ? 0 
                            : Math.Round(double.Parse(orderedTicketsFilms[i].TicketCount) * 100 / totalTicketsFilms[i].PlaceCount))
            });
        }

        return statDto;
    }
}