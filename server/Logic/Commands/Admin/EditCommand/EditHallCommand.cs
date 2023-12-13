using Data;
using Logic.DTO.Admin.ForEditing;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.EditCommand;

/// <summary>
/// Редактирование кинозала.
/// Разрешено только пока зала нет в расписании.
/// </summary>
public class EditHallCommand : IRequest
{
    public int CinemaHallId { get; }
    public string CinemaHallName { get; }
    public int CinemaHallTypeId { get; }
    public int CinemaId { get; }
    public EditHallCommand(EditHallDto editHallDto)
    {
        CinemaHallId = editHallDto.CinemaHallId;
        CinemaHallName = editHallDto.CinemaHallName;
        CinemaHallTypeId = editHallDto.CinemaHallTypeId;
        CinemaId = editHallDto.CinemaId;
    }
}

public class EditHallCommandHandler : IRequestHandler<EditHallCommand>
{
    private readonly ApplicationContext _applicationContext;

    public EditHallCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(EditHallCommand request, CancellationToken cancellationToken)
    {
        // Пытаемся найти кинозал в бд
        var cinemaHall = await _applicationContext.CinemaHalls
            .Where(ch => ch.CinemaHallId == request.CinemaHallId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (cinemaHall == null)
        {
            throw new NotFoundException("Такого кинозала не существует!");
        }

        // Определяем, используется ли кинозал в расписании
        var sessions = await _applicationContext.Sessions
            .Where(s => s.CinemaHallId == request.CinemaHallId)
            .ToListAsync(cancellationToken);

        if (sessions.Count != 0)
        {
            throw new NotAllowedException("Этот зал уже используется в расписании сеансов!");
        }

        // Проверяем, существует ли выбранный пользователем шаблон кинозала
        var cinemaHallType = await _applicationContext.CinemaHallTypes
            .Where(cht => cht.CinemaHallTypeId == request.CinemaHallTypeId)
            .Where(cht => cht.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (cinemaHallType == null)
        {
            throw new NotFoundException("Выбранный шаблон не существует!");
        }

        // Проверяем, существует ли выбранный пользователем кинотеатр
        var cinema = await _applicationContext.Cinemas
            .Where(c => c.CinemaId == request.CinemaId)
            .Where(c => c.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (cinema == null)
        {
            throw new NotFoundException("Выбранный кинотеатр не существует!");
        }

        // Проверяем, нет ли в выбранном кинотеатре зала с таким же названием
        var otherCinemaHall = await _applicationContext.CinemaHalls
            .Where(ch => ch.CinemaHallName.ToLower().Equals(request.CinemaHallName.ToLower()))
            .Where(ch => ch.CinemaId == request.CinemaId)
            .FirstOrDefaultAsync(cancellationToken);

        if (otherCinemaHall != null)
        {
            throw new NotAllowedException("В выбранном кинотеатре уже есть зал с таким названием!");
        }
        
        // Если проверки пройдены -> редактируем
        cinemaHall.CinemaHallName = request.CinemaHallName;
        cinemaHall.CinemaHallTypeId = request.CinemaHallTypeId;
        cinemaHall.CinemaId = request.CinemaId;

        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}