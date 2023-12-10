using Data;
using Logic.DTO.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminTemplateQuery : IRequest<AdminTemplateDto>
{
    public int CinemaHallTypeId { get; }

    public GetAdminTemplateQuery(int cinemaHallTypeId)
    {
        CinemaHallTypeId = cinemaHallTypeId;
    }
}

public class GetAdminTemplateQueryHandler : IRequestHandler<GetAdminTemplateQuery, AdminTemplateDto>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminTemplateQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<AdminTemplateDto> Handle(GetAdminTemplateQuery request, CancellationToken cancellationToken)
    {
        // Сформируем матрицу(CinemaHallDto), учитывая максимальное кол-во мест в зале
        
        // Найдем: rows - max кол-во рядов,
        //         numbers - max кол-во мест в ряду
        var rows = _applicationContext.PlacePositions.Select(p => p.Row).Max();
        var numbers = _applicationContext.PlacePositions.Select(p => p.Number).Max();
        
        // Создаём саму Dto, все ее поля изначально - null
        var adminTemplateDto = new AdminTemplateDto(rows, numbers);
        
        // Находим все места, относящиеся к шаблону
        var places = await _applicationContext.Places
            .Where(p => p.CinemaHallTypeId == request.CinemaHallTypeId)
            .ToListAsync(cancellationToken);
        
        // Для каждого из найденных мест создадим Dto и занесем их в матрицу
        foreach (var place in places)
        {
            var placeType = await _applicationContext.PlaceTypes
                .Where(pt => pt.PlaceTypeId == place.PlaceTypeId)
                .FirstOrDefaultAsync(cancellationToken);
            
            var placePosition = await _applicationContext.PlacePositions
                .Where(ps => ps.PlacePositionId == place.PlacePositionId)
                .FirstOrDefaultAsync(cancellationToken);

            if (placeType == null || placePosition == null)
            {
                throw new Exception("Позиция или Тип места = null");
            }
            
            var placeDto = new AdminPlaceDto
            {
                PlaceTypeId = place.PlaceTypeId,
                Color = placeType.PlaceTypeColor
            };

            var row = placePosition.Row-1;
            var number = placePosition.Number-1;

            adminTemplateDto.AdminPlaceDtos[row][number] = placeDto;
        } //Теперь матрица содержит все реально существующие места
        
        // Заполним оставшиеся элементы матрицы Dto-шками, у которых PlaceTypeId=0
        var defaultPlaceDto = new AdminPlaceDto
        {
            PlaceTypeId = 0,
            Color = "FFFFFF"
        };
        for (var row = 0; row < rows; row++)
        {
            for (var number = 0; number < numbers; number++)
            {
                if (adminTemplateDto.AdminPlaceDtos[row][number].PlaceTypeId == 0)
                {
                    adminTemplateDto.AdminPlaceDtos[row][number] = defaultPlaceDto;
                }
            }
        }

        return adminTemplateDto;
    }
}