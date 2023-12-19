using Data;
using Logic.DTO;
using Logic.DTO.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.User;

public class GetCinemaHallQuery : IRequest<CinemaHallDto>
{
    public int SessionId { get; }

    public GetCinemaHallQuery(int sessionId)
    {
        SessionId = sessionId;
    }
}

public class GetCinemaHallQueryHandler : IRequestHandler<GetCinemaHallQuery, CinemaHallDto>
{
    private readonly ApplicationContext _applicationContext;

    public GetCinemaHallQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<CinemaHallDto> Handle(GetCinemaHallQuery request, CancellationToken cancellationToken)
    {
        // Для начала сформируем матрицу(CinemaHallDto), учитывая максимальное кол-во мест в зале
        
        // Найдем: rows - max кол-во рядов,
        //         numbers - max кол-во мест в ряду
        var rows = _applicationContext.PlacePositions.Select(p => p.Row).Max();
        var numbers = _applicationContext.PlacePositions.Select(p => p.Number).Max();
        // Создаём саму Dto, все ее поля изначально - null
        var cinemaHallDto = new CinemaHallDto(rows, numbers);
        
        //Достанем коэф-ты даты и фильма
        var dataTimeCoefficient = await _applicationContext.Sessions
            .Where(s => s.SessionId == request.SessionId)
            .Select(s => s.DataTimeCoefficient)
            .FirstOrDefaultAsync(cancellationToken);
        var filmCoefficient = await _applicationContext.Sessions
            .Where(s => s.SessionId == request.SessionId)
            .Select(s => s.Film)
            .Select(f => f.FilmCoefficient)
            .FirstOrDefaultAsync(cancellationToken);
        
        
        // Теперь заполним Dto местами, которые действительно существуют в кинозале
        
        // Находим все места, относящиеся к сеансу
        var places = await _applicationContext.Sessions
            .Where(s => s.SessionId == request.SessionId)
            .Select(s => s.CinemaHall).Select(h => h.CinemaHallType)
            .Select(ht => ht.Places).FirstOrDefaultAsync(cancellationToken);
        
        // Найдем id забронированных места
        var placeIds = await _applicationContext.Tickets
            .Where(t => t.SessionId == request.SessionId)
            .Select(t => t.PlaceId).ToListAsync(cancellationToken);
        
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

            // Если место уже забронировано, то сделаем id этого места отрицательным
            int placeTypeId = place.PlaceTypeId;
            if (placeIds.Contains(place.PlaceId))
            {
                placeTypeId *= -1;
            }
            
            var placeDto = new PlaceDto
            {
                PlaceId = place.PlaceId,
                PlaceTypeId = placeTypeId,
                PlaceName = place.PlaceName,
                PlaceTypeName = placeType.PlaceTypeName,
                Color = placeType.PlaceTypeColor,
                Cost = placeType.DefaultCost * dataTimeCoefficient * filmCoefficient,
            };

            var row = placePosition.Row-1;
            var number = placePosition.Number-1;

            cinemaHallDto.PlaceDtos[row][number] = placeDto;
        } //Теперь матрица содержит все реально существующие места
        
        // Заполним оставшиеся элементы матрицы Dto-шками, у которых PlaceTypeId=0
        var defaultPlaceDto = new PlaceDto
        {
            PlaceId = 0,
            PlaceTypeId = 0,
            PlaceName = "default",
            PlaceTypeName = "default",
            Color = "FFFFFF",
            Cost = 0,
        };
        for (int row = 0; row < rows; row++)
        {
            for (int number = 0; number < numbers; number++)
            {
                if (cinemaHallDto.PlaceDtos[row][number].PlaceTypeId == 0)
                {
                    cinemaHallDto.PlaceDtos[row][number] = defaultPlaceDto;
                }
            }
        }

        return cinemaHallDto;
    }
}