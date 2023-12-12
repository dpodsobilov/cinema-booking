using Data;
using Logic.DTO.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminHallQuery : IRequest<IList<AdminHallDto>>
{
    public int CinemaId { get; }

    public GetAdminHallQuery(int cinemaId)
    {
        CinemaId = cinemaId;
    }
}

public class GetAdminHallQueryHandler : IRequestHandler<GetAdminHallQuery, IList<AdminHallDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminHallQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<AdminHallDto>> Handle(GetAdminHallQuery request, CancellationToken cancellationToken)
    {
        var halls = await _applicationContext.CinemaHalls
            .Where(hall => hall.CinemaId == request.CinemaId && hall.IsDeleted == false)
            .Select(hall => new AdminHallDto()
        {
            CinemaHallId = hall.CinemaHallId,
            CinemaHallName = hall.CinemaHallName,
            CinemaHallTypeName = hall.CinemaHallType.CinemaHallTypeName,
            CinemaHallTypeId = hall.CinemaHallTypeId,
            CinemaId = hall.CinemaId
        }).ToListAsync(cancellationToken);
        
        return halls;
    }
}