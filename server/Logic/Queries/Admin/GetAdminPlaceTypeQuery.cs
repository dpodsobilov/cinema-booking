using Data;
using Logic.DTO.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminPlaceTypeQuery : IRequest<IList<AdminGetPlaceTypeDto>> { }

public class GetAdminPlaceTypeQueryHandler : IRequestHandler<GetAdminPlaceTypeQuery, IList<AdminGetPlaceTypeDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminPlaceTypeQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<AdminGetPlaceTypeDto>> Handle(GetAdminPlaceTypeQuery request, CancellationToken cancellationToken)
    {
        var placeType = await _applicationContext.PlaceTypes
            .Where(type => type.IsDeleted == false)
            .Select(type => new AdminGetPlaceTypeDto()
            {
                TypeId = type.PlaceTypeId,
                Name = type.PlaceTypeName,
                Color = type.PlaceTypeColor,
                Cost = type.DefaultCost
            }).ToListAsync(cancellationToken);
        
        return placeType;
    }
}