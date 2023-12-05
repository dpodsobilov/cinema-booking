using Data;
using Logic.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminFilmQuery : IRequest<IList<AdminFilmDto>> { }

public class GetAdminFilmQueryHandler : IRequestHandler<GetAdminFilmQuery, IList<AdminFilmDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminFilmQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<AdminFilmDto>> Handle(GetAdminFilmQuery request, CancellationToken cancellationToken)
    {

        var films = await _applicationContext.Films.Where(film => film.IsDeleted == false)
            .Select(film => new AdminFilmDto
        {
            FilmId = film.FilmId,
            FilmName = film.FilmName
        }).ToListAsync(cancellationToken);
        
        return films;
    }
}