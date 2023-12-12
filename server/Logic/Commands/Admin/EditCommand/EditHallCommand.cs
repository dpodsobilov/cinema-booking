using Data;
using Logic.DTO.Admin.ForEditing;
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
            throw new Exception("Такого кинозала не существует!");
        }

        // Определяем, используется ли кинозал в расписании
        var sessions = await _applicationContext.Sessions
            .Where(s => s.CinemaHallId == request.CinemaHallId)
            .ToListAsync(cancellationToken);

        if (sessions != null)
        {
            throw new Exception("Этот зал уже используется в расписании сеансов!");
        }

        // Проверяем, существует ли выбранный пользователем шаблон кинозала
        var cinemaHallType = await _applicationContext.CinemaHallTypes
            .Where(cht => cht.CinemaHallTypeId == request.CinemaHallTypeId)
            .Where(cht => cht.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (cinemaHallType == null)
        {
            throw new Exception("Выбранный шаблон не существует!");
        }

        // Проверяем, существует ли выбранный пользователем кинотеатр
        var cinema = await _applicationContext.Cinemas
            .Where(c => c.CinemaId == request.CinemaId)
            .Where(c => c.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (cinema == null)
        {
            throw new Exception("Выбранный кинотеатр не существует!");
        }
        
        // Если проверки пройдены -> редактируем
        cinemaHall.CinemaHallName = request.CinemaHallName;
        cinemaHall.CinemaHallTypeId = request.CinemaHallTypeId;
        cinemaHall.CinemaId = request.CinemaId;

        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}