using Data;
using Data.Models;
using Logic.DTO.Admin;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.EditCommand;

/// <summary>
/// Редактирование типа места.
/// </summary>
public class EditPlaceTypeCommand : IRequest
{
    public int TypeId { get; }
    public string Name { get; }
    public string Color { get; }
    public decimal Cost { get; }
    public EditPlaceTypeCommand(AdminPlaceTypeDto adminPlaceTypeDto)
    {
        TypeId = adminPlaceTypeDto.TypeId;
        Name = adminPlaceTypeDto.Name;
        Color = adminPlaceTypeDto.Color;
        Cost = adminPlaceTypeDto.Cost;
    }
}

public class EditPlaceTypeCommandHandler : IRequestHandler<EditPlaceTypeCommand>
{
    private readonly ApplicationContext _applicationContext;

    public EditPlaceTypeCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(EditPlaceTypeCommand request, CancellationToken cancellationToken)
    {
        // Проверяем, существует ли такой тип места
        var placeType = await _applicationContext.PlaceTypes
            .Where(pt => pt.PlaceTypeId == request.TypeId)
            .Where(pt => pt.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (placeType == null)
        {
            throw new NotFoundException("Выбранный тип места не существует!");
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

        var place = places.FirstOrDefault(p => p.PlaceTypeId == request.TypeId);

        if (place != null)
        {
            throw new NotAllowedException("Выбранный тип места используется в шаблонах кинозала!");
        }
        
        // Проверяем, нет ли типа с выбранным названием
        var otherPlaceType = await _applicationContext.PlaceTypes
            .Where(pt => pt.PlaceTypeName.ToLower().Equals(request.Name))
            .Where(pt => pt.PlaceTypeId != request.TypeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (otherPlaceType != null)
        {
            throw new NotAllowedException("Тип места с выбранным названием уже существует!");
        }

        // Если проверки прошли успешно -> редактируем
        placeType.PlaceTypeName = request.Name;
        placeType.PlaceTypeColor = request.Color;
        placeType.DefaultCost = request.Cost;
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}