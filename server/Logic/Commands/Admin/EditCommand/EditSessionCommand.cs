using Data;
using Logic.DTO.Admin.ForEditing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.EditCommand;

/// <summary>
/// Редактирование сеанса.
/// </summary>
public class EditSessionCommand : IRequest
{    
    public int SessionId { get; set; }
    public DateTime  DataTimeSession { get; set; }
    public decimal DataTimeCoefficient { get; set; }
    public int FilmId { get; set; }
    public int CinemaHallId { get; set; }
    public EditSessionCommand(EditSessionDto editSessionDto)
    {
        SessionId = editSessionDto.SessionId;
        DataTimeSession = editSessionDto.DataTimeSession;
        DataTimeCoefficient = editSessionDto.DataTimeCoefficient;
        FilmId = editSessionDto.FilmId;
        CinemaHallId = editSessionDto.CinemaHallId;
    }
}

public class EditSessionCommandHandler : IRequestHandler<EditSessionCommand>
{
    private readonly ApplicationContext _applicationContext;

    public EditSessionCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(EditSessionCommand request, CancellationToken cancellationToken)
    {
        // Проверяем, существует ли такой сеанс
        var session = await _applicationContext.Sessions
            .Where(s => s.SessionId == request.SessionId)
            .Where(s => s.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (session == null)
        {
            throw new Exception("Выбранного сеанса не существует!");
        }

        // Проверяем, нет ли билетов на выбранный сеанс
        var ticket = await _applicationContext.Tickets
            .Where(t => t.SessionId == request.SessionId)
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket != null)
        {
            throw new Exception("На выбранный сеанс уже забронированы билеты!");
        }
        
        // Проверяем, существует ли выбранный фильм
        var film = await _applicationContext.Films
            .Where(f => f.FilmId == request.FilmId)
            .Where(f => f.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (film == null)
        {
            throw new Exception("Выбранный фильм не существует");
        }
        
        // Проверяем, существует ли выбранный кинозал
        var cinemaHall = await _applicationContext.CinemaHalls
            .Where(ch => ch.CinemaHallId == request.CinemaHallId)
            .Where(ch => ch.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (cinemaHall == null)
        {
            throw new Exception("Выбранный кинозал не существует!");
        }
        
        
        
        // Проверяем доступность кинозала в выбранное время
        var cinemaHallIsFree = true;
        
        // Все сеансы в этот день, кроме изменяемого
        var otherSessions = await _applicationContext.Sessions
            .Where(s => s.CinemaHallId == request.CinemaHallId)
            .Where(s => s.IsDeleted == false) // смотрим только "живые" сеансы
            .Where(s => s.DataTimeSession.Date == request.DataTimeSession.Date)
            .Where(s => s.SessionId != request.SessionId)
            .ToListAsync(cancellationToken);
        
        
        // Часы фильма в новом сеансе
        var hours = int.Parse(film.Duration.Split(' ')[0]) ;
        // Минуты фильма в новом сеансе
        var minutes = int.Parse(film.Duration.Split(' ')[2]);

        // Время начала нового сеанса
        var dateTimeStart = request.DataTimeSession;
        // Время окончания нового сеанса (+ 30 минут на уборку зала)
        var dateTimeEnd = request.DataTimeSession.AddHours(hours + 0.5).AddMinutes(minutes);

        // Проходим по всем старым сеансам и сравниваем их периоды на пересечение с новым сеансом
        foreach (var otherSession in otherSessions)
        {
            // фильм старого сеанса
            var otherFilm = await _applicationContext.Films
                .Where(f => f.FilmId == otherSession.FilmId)
                .FirstAsync(cancellationToken);
            
            // часы из длительности фильма
            var otherSessionHours = int.Parse(otherFilm.Duration.Split(' ')[0]) ;
            // минуты из длительности фильма
            var otherSessionMinutes = int.Parse(otherFilm.Duration.Split(' ')[2]);
            
            // Время начала старого сеанса
            var otherSessionStart = otherSession.DataTimeSession;
            // Время окончания старого сеанс (+ 30 минут на уборку зала)
            var otherSessionEnd = otherSession.DataTimeSession.AddHours(otherSessionHours + 0.5).AddMinutes(otherSessionMinutes);
            
            // Если периоды пересекаются -> меняем cinemaHallIsFree
            if (dateTimeStart < otherSessionEnd && otherSessionStart < dateTimeEnd)
            {
                cinemaHallIsFree = false;
            }
        }

        if (!cinemaHallIsFree)
        {
            throw new Exception("Выбранное время накладывается на другой сеанс!");
        }

        // Если проверки пройдены -> редактируем
        session.DataTimeSession = request.DataTimeSession;
        session.DataTimeCoefficient = request.DataTimeCoefficient;
        session.FilmId = request.FilmId;
        session.CinemaHallId = request.CinemaHallId;

        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}