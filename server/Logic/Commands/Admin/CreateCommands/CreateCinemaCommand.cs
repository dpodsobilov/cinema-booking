using Data;
using Data.Models;
using Logic.DTO.Admin.ForCreating;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.CreateCommands;

/// <summary>
/// Команда для создания нового кинотеатра
/// </summary>
public class CreateCinemaCommand : IRequest
{
    public string CinemaName { get; }
    public string Address { get; }

    public CreateCinemaCommand(CreationCinemaDto creationCinemaDto)
    {
        CinemaName = creationCinemaDto.CinemaName;
        Address = creationCinemaDto.Address;
    }
}

public class CreateCinemaCommandHandler : IRequestHandler<CreateCinemaCommand>
{
    private readonly ApplicationContext _applicationContext;

    public CreateCinemaCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(CreateCinemaCommand request, CancellationToken cancellationToken)
    {
        // Пытаемся найти такой экзмепляр в бд
        // сравнение по названию кинотеатра
        var oldCinema = await  _applicationContext.Cinemas
            .Where(c => c.CinemaName.ToLower().Equals(request.CinemaName.ToLower()))
            .FirstOrDefaultAsync(cancellationToken);

        // Если такой есть и он удалён - восстанавливаем
        if (oldCinema != null && oldCinema.IsDeleted)
        {
            oldCinema.IsDeleted = false;
            // Задаём новый адрес
            oldCinema.Address = request.Address;
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }
        // Если такого нет - добавляем в бд
        if (oldCinema == null)
        {
            // Создали новый экземпляр создаваемого объекта
            var newCinema = new Cinema()
            {
                CinemaName = request.CinemaName,
                Address = request.Address
            };
            await _applicationContext.Cinemas.AddAsync(newCinema, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }

        // Иначе юзеру придёт ошибка
        throw new NotAllowedException("Кинотеатр с выбранным названием уже существует!");
    }
}