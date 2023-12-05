using Data;
using Logic.DTO;
using Logic.DTO.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminCinemaQuery : IRequest<IList<AdminCinemaDto>> { }

public class GetAdminCinemaQueryHandler : IRequestHandler<GetAdminCinemaQuery, IList<AdminCinemaDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminCinemaQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<AdminCinemaDto>> Handle(GetAdminCinemaQuery request, CancellationToken cancellationToken)
    {
        var cinemas = await _applicationContext.Cinemas
            .Where(cinema => cinema.IsDeleted == false)
            .Select(cinema => new AdminCinemaDto()
        {
            CinemaId = cinema.CinemaId,
            CinemaName = cinema.CinemaName,
            CinemaAddress = cinema.Address
        }).ToListAsync(cancellationToken);
        
        return cinemas;
    }
}