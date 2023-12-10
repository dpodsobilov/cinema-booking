using Data;
using Data.Models;
using Logic.DTO.Admin.ForCreating;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.CreateCommands;

/// <summary>
/// Команда для создания шаблона кинозала,
/// добавление в таблицы CinemaHallType, Places
/// </summary>
public class CreateTemplateCommand : IRequest
{
    public string CinemaHallTypeName { get; }
    public List<List<int>> TemplatePlaceTypes { get; }
    
    public CreateTemplateCommand(CreationTemplateDto creationTemplateDto)
    {
        CinemaHallTypeName = creationTemplateDto.CinemaHallTypeName;
        TemplatePlaceTypes = creationTemplateDto.TemplatePlaceTypes;
    }
}

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand>
{
    private readonly ApplicationContext _applicationContext;

    public CreateTemplateCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        // Если какой-то тип места не существует -> выбрасываем ошибку
        if (! await CheckExistOfPlaceTypes(request, cancellationToken))
        {
            throw new Exception("Тип места, указанный в шаблоне, не существует");
        }
        
        // Пытаемся найти такой шаблон в бд
        var oldCinemaHallType = await  _applicationContext.CinemaHallTypes
            // сравнение по названию
            .Where(cht => cht.CinemaHallTypeName.ToLower().Equals(request.CinemaHallTypeName.ToLower()))
            // достаём только существующие шаблоны
            .Where(cht => cht.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        
        // Если такого нет - добавляем в бд
        if (oldCinemaHallType == null)
        {
            // Создали новый экземпляр
            var newCinemaHallType = new CinemaHallType
            {
                CinemaHallTypeName = request.CinemaHallTypeName
            };
            await _applicationContext.CinemaHallTypes.AddAsync(newCinemaHallType, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            
            // Добавили ему места
            var cinemaHallType = await _applicationContext.CinemaHallTypes
                .Where(cht => cht.CinemaHallTypeName.Equals(request.CinemaHallTypeName))
                .Where(cht => cht.IsDeleted == false)
                .FirstAsync(cancellationToken);
            
            await AddPlacesForCinemaHallType(cinemaHallType, request, cancellationToken);
            return;
        }

        // Иначе юзеру придёт ошибка
        throw new Exception("Такой шаблон уже существует!");
        
    }

    /// <summary>
    /// Проверяет существование типов мест из CreateTemplateCommand
    /// </summary>
    /// <returns>True - если все типы мест существуют</returns>
    private async Task<bool> CheckExistOfPlaceTypes(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        // Все типы, которые есть в шаблоне
        HashSet<int> types = new HashSet<int>();
        foreach (var templatePlaceTypes in request.TemplatePlaceTypes)
        {
            foreach (var type in templatePlaceTypes)
            {
                types.Add(type);
            }
        }

        // Т.к. матрица максимального размера, необходимо удалить несуществующие места(приходят с типом = 0)
        types.Remove(0);

        // Проверяем существование типов мест
        foreach (var type in types)
        {
            var placeType = await _applicationContext.PlaceTypes
                .Where(pt => pt.PlaceTypeId == type)
                .FirstOrDefaultAsync(cancellationToken);

            // Если какой-то тип не существует -> вернём false
            if (placeType == null)
            {
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Добавляет места к указанному шаблону кинозала
    /// </summary>
    private async Task AddPlacesForCinemaHallType(
        CinemaHallType cinemaHallType,
        CreateTemplateCommand request, 
        CancellationToken cancellationToken)
    {
        // Для именования
        var names = new [] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        
        // Список новых мест
        var places = new List<Place>();
        
        // Проверка на соответствие размера матриц опущена - предполагаем, что придёт правильная

        var row = 0; // текущий ряд в исходной матрице
        var number = 0; // текущее место в исходной матрице
        var rowForName = 0; // текущий ряд для юзера
        var numberForName = 0; // текущее место для юзера
        
        foreach (var templatePlaceTypes in request.TemplatePlaceTypes)
        {
            row++; // взяли новый ряд в матрице
            number = 0; // обнулили номер места в матрице
            numberForName = 0; // обнулили номер места для юзера
            
            foreach (var templatePlaceType in templatePlaceTypes)
            {
                number++; // идя по ряду, увеличиваем номер места в матрице

                if (templatePlaceType == 0) continue; // нас интересуют ТОЛЬКО существующие места
                
                if (numberForName == 0) // если это первое найденное место в ряду
                {
                    rowForName++; // то увеличим ряд для юзера
                }
                numberForName++; // увеличим номер места для юзера
                    
                // Теперь создадим место
                var place = new Place
                {
                    PlaceTypeId = templatePlaceType,
                    CinemaHallTypeId = cinemaHallType.CinemaHallTypeId,
                    PlacePositionId = await _applicationContext.PlacePositions
                        .Where(pp => pp.Row == row && pp.Number == number)
                        .Select(pp => pp.PlacePositionId)
                        .FirstAsync(cancellationToken), // предполагаем, что позиция точно есть
                    PlaceName = names[rowForName-1] + numberForName // формирование названия места
                };
                // добавим его в лист мест
                places.Add(place);
            }
        }
        // Добавим полученный лист мест в бд
        await _applicationContext.Places.AddRangeAsync(places, cancellationToken);
        
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
    
}