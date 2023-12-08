using Data;
using Data.Models;
using Logic.DTO.Admin.ForCreating;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.CreateCommands;

public class CreateCinemaHallCommand : IRequest
{
    public string CinemaHallName { get; set; }
    public int CinemaHallTypeId { get; set; }
    public int CinemaId { get; set; }
    public CreateCinemaHallCommand(CreationCinemaHallDto creationCinemaHallDto)
    {
        CinemaHallName = creationCinemaHallDto.CinemaHallName;
        CinemaHallTypeId = creationCinemaHallDto.CinemaHallTypeId;
        CinemaId = creationCinemaHallDto.CinemaId;
    }
}

public class CreateCinemaHallCommandHandler : IRequestHandler<CreateCinemaHallCommand>
{
    private readonly ApplicationContext _applicationContext;

    public CreateCinemaHallCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(CreateCinemaHallCommand request, CancellationToken cancellationToken)
    {
        // Проверим наличие шаблона кинозала и кинотеатра
        var cinemaHallType = await _applicationContext.CinemaHallTypes
            .Where(cht => cht.CinemaHallTypeId == request.CinemaHallTypeId)
            .FirstOrDefaultAsync(cancellationToken);
        var cinema = await _applicationContext.Cinemas
            .Where(c => c.CinemaId == request.CinemaId)
            .FirstOrDefaultAsync(cancellationToken);
        
        // Если таких таких нет -> выдаём ошибку
        if (cinemaHallType == null || cinema == null)
        {
            throw new Exception("Не существует такой кинотеатр и/или шаблон кинозала!");
        }

        // Если же шаблон и кинотеатр нашлись в бд -> продолжаем создание кинозала
        // Пытаемся найти такой экземпляр кинозала в бд
        var oldCinemaHall = await  _applicationContext.CinemaHalls
            // сравнение по названию кинозала
            .Where(ch => ch.CinemaHallName.ToLower().Equals(request.CinemaHallName.ToLower()))
            // сравнение по шаблону кинозала
            .Where(ch => ch.CinemaHallTypeId == request.CinemaHallTypeId)
            // сравнение по привязке к кинотеатру
            .Where(ch => ch.CinemaId == request.CinemaId)
            .FirstOrDefaultAsync(cancellationToken);

        // Если такой есть и он удалён - восстанавливаем
        if (oldCinemaHall != null && oldCinemaHall.IsDeleted)
        {
            oldCinemaHall.IsDeleted = false;
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }
        // Если такого нет - добавляем в бд
        if (oldCinemaHall == null)
        {
            // Создали новый экземпляр
            var newCinemaHall = new CinemaHall
            {
                CinemaHallName = request.CinemaHallName,
                CinemaHallTypeId = request.CinemaHallTypeId,
                CinemaId = request.CinemaId
            };
            await _applicationContext.CinemaHalls.AddAsync(newCinemaHall, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }

        // Иначе юзеру придёт ошибка
        throw new Exception("Есть же уже такой кинотеатр!");
    }
}