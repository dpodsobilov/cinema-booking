using Data;
using Logic.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.GetSchedule;

public class GetScheduleQueryHandler : IRequestHandler<GetScheduleQuery, IList<FilmScheduleDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetScheduleQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IList<FilmScheduleDto>> Handle(GetScheduleQuery request, CancellationToken cancellationToken)
    {
        var filmSchedules = await _applicationContext.Sessions
            .Where(session => session.FilmId == request.FilmId)
            .Include(session => session.CinemaHall)
            .ThenInclude(cinemaHall => cinemaHall.Cinema)
            .Select(session => new FilmScheduleDto
            {
                CinemaId = session.CinemaHall.Cinema.CinemaId,
                CinemaName = session.CinemaHall.Cinema.CinemaName,
                CinemaHallId = session.CinemaHall.CinemaHallId,
                CinemaHallName = session.CinemaHall.CinemaHallName,
                SessionId = session.SessionId,
                DataTimeSession = session.DataTimeSession
            })
            .ToListAsync(cancellationToken);

        return filmSchedules;
    }
}