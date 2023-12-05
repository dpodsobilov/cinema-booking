using Data;
using Logic.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminGenreQuery : IRequest<IList<AdminGenreDto>> { }

public class GetAdminGenreQueryHandler : IRequestHandler<GetAdminGenreQuery, IList<AdminGenreDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminGenreQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<AdminGenreDto>> Handle(GetAdminGenreQuery request, CancellationToken cancellationToken)
    {

        var genres = await _applicationContext.Genres.Where(genre => genre.IsDeleted == false)
            .Select(genre => new AdminGenreDto
        {
            GenreId = genre.GenreId,
            GenreName = genre.GenreName
        }).ToListAsync(cancellationToken);
        
        return genres;
    }
}