using Data;
using Logic.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries;

public class GetUserTicketsQuery : IRequest<AllUsersTicketsDto>
{
    public int UserId { get; }
    
    public GetUserTicketsQuery(int userId)
    {
        UserId = userId;
    }
}

public class GetUserTicketsQueryHandler : IRequestHandler<GetUserTicketsQuery, AllUsersTicketsDto>
{
    private readonly ApplicationContext _applicationContext;

    public GetUserTicketsQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<AllUsersTicketsDto> Handle(GetUserTicketsQuery request, CancellationToken cancellationToken)
    {
        
        var pastTickets = await _applicationContext.Tickets.Where(ticket => ticket.UserId == request.UserId)
            .Where(ticket => ticket.Session.DataTimeSession < DateTime.Now)
            .Select(ticket => new UserTicketsDto
            {
                TicketId = ticket.TicketId,
                FilmName = ticket.Session.Film.FilmName,
                CinemaName = ticket.Session.CinemaHall.Cinema.CinemaName,
                CinemaHallName = ticket.Session.CinemaHall.CinemaHallName,
                Date = ticket.Session.DataTimeSession,
                PlaceName = ticket.Place.PlaceName,
                PlaceTypeName = ticket.Place.PlaceType.PlaceTypeName
            }).ToListAsync(cancellationToken);
        
        var upcomingTickets = await _applicationContext.Tickets.Where(ticket => ticket.UserId == request.UserId)
            .Where(ticket => ticket.Session.DataTimeSession >= DateTime.Now)
            .Select(ticket => new UserTicketsDto
            {
                TicketId = ticket.TicketId,
                FilmName = ticket.Session.Film.FilmName,
                CinemaName = ticket.Session.CinemaHall.Cinema.CinemaName,
                CinemaHallName = ticket.Session.CinemaHall.CinemaHallName,
                Date = ticket.Session.DataTimeSession,
                PlaceName = ticket.Place.PlaceName,
                PlaceTypeName = ticket.Place.PlaceType.PlaceTypeName
            }).ToListAsync(cancellationToken);

        var tickets = new AllUsersTicketsDto
        {
            upcomingTickets = upcomingTickets,
            pastTickets = pastTickets
        };
        return tickets;
    }
}