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

        //var sessions = await _applicationContext.Sessions
        //    .Select(session => session)
        //    .Distinct().ToListAsync(cancellationToken);

        //foreach(var session in sessions)
        //{
        //    var orderedTickets = await _applicationContext.Sessions.Count().;
        //}

        var result = await _applicationContext.Films
            .GroupJoin(_applicationContext.Sessions, film => film.FilmId, session => session.FilmId, (film, sessions) => new { Film = film, Sessions = sessions })
            .SelectMany(x => x.Sessions.DefaultIfEmpty(), (film, session) => new { Film = film.Film, Session = session })
            .GroupJoin(_applicationContext.Tickets, x => x.Session.SessionId, ticket => ticket.SessionId, (x, tickets) => new { Film = x.Film, Tickets = tickets })
            .SelectMany(x => x.Tickets.DefaultIfEmpty(), (x, ticket) => new { Film = x.Film, Ticket = ticket })
            .GroupBy(x => x.Film.FilmName)
            .Select(group => new { FilmName = group.Key, TicketCount = group.Count() })
            .ToListAsync(cancellationToken);

        var result2 = await _applicationContext.Films
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

        foreach (var item in result)
        {
            Console.WriteLine($"{item.FilmName}: {item.TicketCount} places");
        }

        Console.WriteLine("*************************************************");

        foreach (var item in result2)
        {
            Console.WriteLine($"{item.FilmName}: {item.PlaceCount} places");
        }

        var statDto = new List<AdminStatDto>();

        for (int i  = 0; i < result.Count(); ++i)
        {
            statDto.Add(new AdminStatDto
            {
                FilmName = result[i].FilmName,
                OrderedTickets = result[i].TicketCount,
                TotalTickets = result2[i].PlaceCount
            });
        }

        return statDto;

        //var tickets = await _applicationContext.Tickets
        //   .GroupBy(ticket => ticket.Session.Film.FilmName)
        //   .Select(group => new AdminStatDto
        //   {

        //   })
        //   .ToListAsync(cancellationToken);
        //var orderedTickets = await _applicationContext.Films
        //    .Count(film => film.session)

        //var stats = await _applicationContext.Films.Select(film => new AdminFilmDto
        //{
        //    FilmId = film.FilmId,
        //    FilmName = film.FilmName
        //}).ToListAsync(cancellationToken);

        //await _applicationContext.Tickets
        //    .Count(ticket => )
        //    .Select(ticket => new AdminStatDto
        //    {
        //        FilmName = ticket.Session.Film.FilmName,
        //        OrderedTickets = ticket.
        //    }).ToListAsync(cancellationToken);

        //return stats;
    }
}