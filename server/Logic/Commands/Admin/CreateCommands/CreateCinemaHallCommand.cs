using Data;
using Data.Models;
using Logic.DTO.Admin.ForCreating;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.CreateCommands;

public class CreateCinemaHallCommand : IRequest
{
    public string CinemaHallName { get; }
    public int CinemaHallTypeId { get; }
    public int CinemaId { get; }
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
        // Проверим, существует ли выбранный шаблон кинозала
        var cinemaHallType = await _applicationContext.CinemaHallTypes
            .Where(cht => cht.CinemaHallTypeId == request.CinemaHallTypeId)
            .Where(cht => cht.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (cinemaHallType == null)
        {
            throw new NotFoundException("Выбранный шаблон кинозала не существует!");
        }
        
        // Проверим, существует ли выбранный кинотеатр
        var cinema = await _applicationContext.Cinemas
            .Where(c => c.CinemaId == request.CinemaId)
            .Where(c => c.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (cinema == null)
        {
            throw new NotFoundException("Выбранный кинотеатр не существует!");
        }
        
        // Проверим существование кинозала
        var oldCinemaHall = await _applicationContext.CinemaHalls
            // сравнение по названию кинозала
            .Where(ch => ch.CinemaHallName.ToLower().Equals(request.CinemaHallName.ToLower()))
            // сравнение по привязке к кинотеатру
            .Where(ch => ch.CinemaId == request.CinemaId)
            .FirstOrDefaultAsync(cancellationToken);

        // Если такой есть и он удалён - восстанавливаем
        if (oldCinemaHall != null && oldCinemaHall.IsDeleted)
        {
            oldCinemaHall.IsDeleted = false;
            // Обновляем шаблон кинозала
            oldCinemaHall.CinemaHallTypeId = request.CinemaHallTypeId;
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }
        // Если такого нет - создаём
        if (oldCinemaHall == null)
        {
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
        throw new NotAllowedException("Кинозал с выбранным названием уже существует в кинотеатре!");
    }
}