using Data;
using Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin;

/// <summary>
/// Удаление типа места.
/// </summary>
public class DeletePlaceTypeCommand : IRequest
{
    public int PlaceTypeId { get; }

    public DeletePlaceTypeCommand(int placeTypeId)
    {
        PlaceTypeId = placeTypeId;
    }
}

public class DeletePlaceTypeCommandHandler : IRequestHandler<DeletePlaceTypeCommand>
{
    private readonly ApplicationContext _applicationContext;

    public DeletePlaceTypeCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(DeletePlaceTypeCommand request, CancellationToken cancellationToken)
    {
        // Проверяем, существует ли такой тип места
        var placeType = await _applicationContext.PlaceTypes
            .Where(pt => pt.PlaceTypeId == request.PlaceTypeId)
            .Where(pt => pt.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (placeType == null)
        {
            throw new Exception("Выбранный тип места не существует!");
        }

        // Проверяем, не используется ли тип места в НЕудалённых шаблонах (точнее в их местах)
        var places = new List<Place>();
        
        var placesList = await _applicationContext.CinemaHallTypes
            .Where(cht => cht.IsDeleted == false)
            .Select(cht => cht.Places)
            .ToListAsync(cancellationToken);

        foreach (var placesCollection in placesList)
        {
            places.AddRange(placesCollection);
        }

        var place = places.FirstOrDefault(p => p.PlaceTypeId == request.PlaceTypeId);

        if (place != null)
        {
            throw new Exception("Выбранный тип места используется в шаблонах кинозала!");
        }

        // Если проверки пройдены успешно -> удаляем
        placeType.IsDeleted = true;
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}