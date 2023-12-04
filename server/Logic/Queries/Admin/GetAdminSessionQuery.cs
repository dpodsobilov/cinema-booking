using Data;
using Logic.DTO.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminSessionQuery : IRequest<IList<AdminSessionDto>>
{
}

public class GetAdminSessionQueryHandler : IRequestHandler<GetAdminSessionQuery, IList<AdminSessionDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminSessionQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<AdminSessionDto>> Handle(GetAdminSessionQuery request, CancellationToken cancellationToken)
    {
        var sessions = await _applicationContext.Sessions
            .Where(session => session.IsDeleted == false)
            .OrderByDescending(session => session.CinemaHall.Cinema.CinemaName)
            .ThenByDescending(session => session.CinemaHall.CinemaHallName)
            .ThenByDescending(session => session.DataTimeSession.Date)
            .ThenByDescending(session => session.DataTimeSession.Date.Hour)
            .Select(session => new AdminSessionDto()
        {
            SessionId = session.SessionId,
            FilmName = session.Film.FilmName,
            CinemaName = session.CinemaHall.Cinema.CinemaName,
            CinemaHallName = session.CinemaHall.CinemaHallName,
            SessionDate = session.DataTimeSession.ToString("dd-MM-yyyy"),
            SessionTime = session.DataTimeSession.ToString("HH:mm"),
        }).ToListAsync(cancellationToken);
        
        return sessions;
    }
}