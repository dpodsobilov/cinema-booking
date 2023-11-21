using Logic.DTO;
using MediatR;

namespace Logic.Queries.GetSchedule;

public class GetScheduleQuery  : IRequest<IList<FilmScheduleDto>>
{
    public int FilmId { get; }

    public GetScheduleQuery(int filmId)
    {
        FilmId = filmId;
    }
}