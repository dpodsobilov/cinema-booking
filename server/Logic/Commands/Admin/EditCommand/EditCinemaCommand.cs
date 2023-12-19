using Data;
using Data.Models;
using Logic.DTO.Admin;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.EditCommand;

/// <summary>
/// Редактирование кинотеатра
/// </summary>
public class EditCinemaCommand : IRequest
{
    public int CinemaId { get; }
    public string CinemaName { get; }
    public string CinemaAddress { get; }
    public EditCinemaCommand(AdminCinemaDto adminCinemaDto)
    {
        CinemaId = adminCinemaDto.CinemaId;
        CinemaName = adminCinemaDto.CinemaName;
        CinemaAddress = adminCinemaDto.CinemaAddress;
    }
}

public class EditCinemaCommandHandler : IRequestHandler<EditCinemaCommand>
{
    private readonly ApplicationContext _applicationContext;

    public EditCinemaCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(EditCinemaCommand request, CancellationToken cancellationToken)
    {
        // Проверяем, существует ли выбранный кинотеатр
        var cinema = await _applicationContext.Cinemas
            .Where(c => c.CinemaId == request.CinemaId)
            .Where(c => c.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (cinema == null)
        {
            throw new NotFoundException("Выбранный кинотеатр не существует!");
        }
        
        // Проверяем, не используется ли кинотеатр в расписании
        var sessions = new List<Session>();
        
        var sessionsList = await _applicationContext.CinemaHalls
            .Where(ch => ch.CinemaId == request.CinemaId)
            .Select(ch => ch.Sessions)
            .ToListAsync(cancellationToken);
        
        foreach (var sessionCollection in sessionsList)
        {
            sessions.AddRange(sessionCollection);
        }

        if (sessions.Count != 0)
        {
            throw new NotAllowedException("Выбранный кинотеатр уже используется в расписании!");
        }
        
        // Проверяем, нет ли другого кинотеатра с таким же названием
        var otherCinema = await _applicationContext.Cinemas
            .Where(c => c.CinemaName.ToLower().Equals(request.CinemaName.ToLower()))
            .Where(c => c.CinemaId != request.CinemaId)
            .Where(c => c.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (otherCinema != null)
        {
            throw new NotAllowedException("Кинотеатр с выбранным названием уже существует!");
        }

        // Если все проверки пройдены -> редактируем
        cinema.CinemaName = request.CinemaName;
        cinema.Address = request.CinemaAddress;
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}