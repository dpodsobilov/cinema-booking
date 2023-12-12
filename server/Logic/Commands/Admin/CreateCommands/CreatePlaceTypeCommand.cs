using Data;
using Data.Models;
using Logic.DTO.Admin.ForCreating;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.CreateCommands;

/// <summary>
/// Создание типа места.
/// </summary>
public class CreatePlaceTypeCommand : IRequest
{
    public string Name { get; }
    public string Color { get; }
    public decimal Cost { get; }
    public CreatePlaceTypeCommand(CreationPlaceTypeDto createPlaceTypeCommand)
    {
        Name = createPlaceTypeCommand.Name;
        Color = createPlaceTypeCommand.Color;
        Cost = createPlaceTypeCommand.Cost;
    }
}

public class CreatePlaceTypeCommandHandler : IRequestHandler<CreatePlaceTypeCommand>
{
    private readonly ApplicationContext _applicationContext;

    public CreatePlaceTypeCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(CreatePlaceTypeCommand request, CancellationToken cancellationToken)
    {
        // Проверяем, существует ли тип места с таким названием
        var oldPlaceType = await _applicationContext.PlaceTypes
            .Where(pt => pt.PlaceTypeName.ToLower().Equals(request.Name))
            .FirstOrDefaultAsync(cancellationToken);

        // Если такой есть и он удалён -> восстанавливаем
        if (oldPlaceType != null && oldPlaceType.IsDeleted)
        {
            oldPlaceType.IsDeleted = false;
            oldPlaceType.PlaceTypeColor = request.Color;
            oldPlaceType.DefaultCost = request.Cost;
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }
        // Если такого нет -> создаём
        if (oldPlaceType == null)
        {
            // Создали новый тип места
            var placeType = new PlaceType
            {
                PlaceTypeName = request.Name,
                PlaceTypeColor = request.Color,
                DefaultCost = request.Cost
            };
            await _applicationContext.PlaceTypes.AddAsync(placeType, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }

        // Иначе юзеру придёт ошибка
        throw new Exception("Тип места с таким названием уже существует!");
    }
}