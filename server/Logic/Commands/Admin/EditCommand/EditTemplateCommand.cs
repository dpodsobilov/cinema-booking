using Data;
using Data.Models;
using Logic.Commands.Admin.CreateCommands;
using Logic.DTO.Admin.ForEditing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.EditCommand;

public class EditTemplateCommand : IRequest
{
    public int CinemaHallTypeId { get; set; }
    public string CinemaHallTypeName { get; }
    public List<List<int>> TemplatePlaceTypes { get; }
    
    public EditTemplateCommand(EditTemplateDto editTemplateDto)
    {
        CinemaHallTypeId = editTemplateDto.CinemaHallTypeId;
        CinemaHallTypeName = editTemplateDto.CinemaHallTypeName;
        TemplatePlaceTypes = editTemplateDto.TemplatePlaceTypes;
    }
}

public class EditTemplateCommandHandler : IRequestHandler<EditTemplateCommand>
{
    private readonly ApplicationContext _applicationContext;

    public EditTemplateCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(EditTemplateCommand request, CancellationToken cancellationToken)
    {
        // Если какой-то тип места не существует -> выбрасываем ошибку
        if (! await CheckExistOfPlaceTypes(request, cancellationToken))
        {
            throw new Exception("Тип места, указанный в шаблоне, не существует");
        }
        
        // Пытаемся найти такой шаблон в бд
        var cinemaHallType = await  _applicationContext.CinemaHallTypes
            // сравнение по Id
            .Where(cht => cht.CinemaHallTypeId == request.CinemaHallTypeId)
            .FirstOrDefaultAsync(cancellationToken);
        
        // Если такого нет - кидаем ошибку
        if (cinemaHallType == null)
        {
            throw new Exception("Такого шаблона не существует!");
        }

        // Определяем, используется ли шаблон в расписании
        var sessions = new List<Session>();
        
        var cinemaHalls = await _applicationContext.CinemaHalls
            .Where(ch => ch.CinemaHallTypeId == request.CinemaHallTypeId)
            .ToListAsync(cancellationToken);

        foreach (var cinemaHall in cinemaHalls)
        {
            var sessionList = await _applicationContext.Sessions
                .Where(s => s.CinemaHallId == cinemaHall.CinemaHallId)
                .ToListAsync(cancellationToken);
            sessions.AddRange(sessionList);
        }
        // Если используется -> кидаем ошибку
        if (sessions == null)
        {
            throw new Exception("Этот шаблон уже используется в расписании сеансов!");
        }
        
        // Само редактирование
        cinemaHallType.CinemaHallTypeName = request.CinemaHallTypeName;
        await AddPlacesForCinemaHallType(cinemaHallType, request, cancellationToken);
        
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Проверяет существование типов мест из CreateTemplateCommand
    /// </summary>
    /// <returns>True - если все типы мест существуют</returns>
    private async Task<bool> CheckExistOfPlaceTypes(EditTemplateCommand request, CancellationToken cancellationToken)
    {
        // Все типы, которые есть в шаблоне
        var types = new HashSet<int>();
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
        EditTemplateCommand request, 
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