using Data;
using Logic.DTO.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminTemplatePlacesTypesQuery : IRequest<IList<AdminGetTemplatePlaceTypeDto>> { }

public class GetAdminPlacesTypesQueryHandler : IRequestHandler<GetAdminTemplatePlacesTypesQuery, IList<AdminGetTemplatePlaceTypeDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminPlacesTypesQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<AdminGetTemplatePlaceTypeDto>> Handle(GetAdminTemplatePlacesTypesQuery request, CancellationToken cancellationToken)
    {
        var placesTypes = await _applicationContext.PlaceTypes
            .Where(type => type.IsDeleted == false)
            .Select(type => new AdminGetTemplatePlaceTypeDto()
            {
                PlaceTypeId = type.PlaceTypeId,
                PlaceTypeName = type.PlaceTypeName,
                PlaceTypeColor = type.PlaceTypeColor
            }).ToListAsync(cancellationToken);
        
        return placesTypes;
    }
}