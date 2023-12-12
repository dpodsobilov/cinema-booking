using Data;
using Data.Models;
using Logic.DTO.Admin.ForCreating;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.CreateCommands;

/// <summary>
/// Создание сеанса
/// </summary>
public class CreateSessionCommand : IRequest
{
    public DateTime  DataTimeSession { get;}
    public decimal DataTimeCoefficient { get;}
    public int FilmId { get;}
    public int CinemaHallId { get;}
    public CreateSessionCommand(CreationSessionDto creationSessionDto)
    {
        DataTimeSession = creationSessionDto.DataTimeSession;
        DataTimeCoefficient = creationSessionDto.DataTimeCoefficient;
        FilmId = creationSessionDto.FilmId;
        CinemaHallId = creationSessionDto.CinemaHallId;
    }
}

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand>
{
    private readonly ApplicationContext _applicationContext;

    public CreateSessionCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        // Проверяем, существует ли выбранный фильм
        var film = await _applicationContext.Films
            .Where(f => f.FilmId == request.FilmId)
            .Where(f => f.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (film == null)
        {
            throw new NotFoundException("Выбранный фильм не существует");
        }
        
        // Проверяем, существует ли выбранный кинозал
        var cinemaHall = await _applicationContext.CinemaHalls
            .Where(ch => ch.CinemaHallId == request.CinemaHallId)
            .Where(ch => ch.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (cinemaHall == null)
        {
            throw new NotFoundException("Выбранный кинозал не существует!");
        }
        
        // Проверяем кол-во сеансов в кинозале в выбранный день (должно быть меньше 5)
        var sessions = await _applicationContext.Sessions
            .Where(s => s.CinemaHallId == request.CinemaHallId)
            .Where(s => s.IsDeleted == false) // смотрим только "живые" сеансы
            .Where(s => s.DataTimeSession.Date == request.DataTimeSession.Date)
            .ToListAsync(cancellationToken);

        if (sessions.Count >= 5)
        {
            throw new NotAllowedException("В кинозале уже есть 5 сеансов!");
        }
        
        
        // Проверяем доступность кинозала в выбранное время
        var cinemaHallIsFree = true;
        
        // Часы фильма в новом сеансе
        var hours = int.Parse(film.Duration.Split(' ')[0]) ;
        // Минуты фильма в новом сеансе
        var minutes = int.Parse(film.Duration.Split(' ')[2]);

        // Время начала нового сеанса
        var dateTimeStart = request.DataTimeSession;
        // Время окончания нового сеанса (+ 30 минут на уборку зала)
        var dateTimeEnd = request.DataTimeSession.AddHours(hours + 0.5).AddMinutes(minutes);

        // Проходим по всем старым сеансам и сравниваем их периоды на пересечение с новым сеансом
        foreach (var session in sessions)
        {
            // фильм старого сеанса
            var oldFilm = await _applicationContext.Films
                .Where(f => f.FilmId == session.FilmId)
                .FirstAsync(cancellationToken);
            
            // часы из длительности фильма
            var sessionHours = int.Parse(oldFilm.Duration.Split(' ')[0]) ;
            // минуты из длительности фильма
            var sessionMinutes = int.Parse(oldFilm.Duration.Split(' ')[2]);
            
            // Время начала старого сеанса
            var sessionStart = session.DataTimeSession;
            // Время окончания старого сеанс (+ 30 минут на уборку зала)
            var sessionEnd = session.DataTimeSession.AddHours(sessionHours + 0.5).AddMinutes(sessionMinutes);
            
            // Если периоды пересекаются -> меняем cinemaHallIsFree
            if (dateTimeStart < sessionEnd && sessionStart < dateTimeEnd)
            {
                cinemaHallIsFree = false;
            }
        }

        if (!cinemaHallIsFree)
        {
            throw new NotAllowedException("Выбранное время накладывается на другой сеанс!");
        }
        
        
        // Если проверки пройдены -> добавляем сеанс
        await _applicationContext.Sessions.AddAsync(
            new Session
            {
                DataTimeSession = request.DataTimeSession,
                DataTimeCoefficient = request.DataTimeCoefficient,
                FilmId = request.FilmId,
                CinemaHallId = request.CinemaHallId
            }, cancellationToken);

        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}