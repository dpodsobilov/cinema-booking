using Data;
using Logic.DTO.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminTemplatesQuery : IRequest<IList<AdminTemplatesDto>>
{
}

public class GetAdminTemplatesQueryHandler : IRequestHandler<GetAdminTemplatesQuery, IList<AdminTemplatesDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminTemplatesQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<AdminTemplatesDto>> Handle(GetAdminTemplatesQuery request, CancellationToken cancellationToken)
    {
        var templates = await _applicationContext.CinemaHallTypes
            .Where(template => template.IsDeleted == false)
            .Select(template => new AdminTemplatesDto()
            {
                TemplateId = template.CinemaHallTypeId,
                TemplateName = template.CinemaHallTypeName
            }).ToListAsync(cancellationToken);
        return templates;
    }
}