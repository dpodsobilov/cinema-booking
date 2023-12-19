using Data;
using Data.Models;
using Logic.DTO.Admin.ForCreating;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.CreateCommands;

public class CreateGenreCommand : IRequest
{
    public string Name { get; }

    public CreateGenreCommand(CreationGenreDto creationGenreDto)
    {
        Name = creationGenreDto.Name;
    }
}

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand>
{
    private readonly ApplicationContext _applicationContext;

    public CreateGenreCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        // Пытаемся найти такой экзмепляр в бд
        // сравнение по названию жанра
        var oldGenre = await  _applicationContext.Genres
            .Where(g => g.GenreName.ToLower().Equals(request.Name.ToLower()))
            .FirstOrDefaultAsync(cancellationToken);

        // Если такой есть и он удалён - восстанавливаем
        if (oldGenre != null && oldGenre.IsDeleted)
        {
            oldGenre.IsDeleted = false;
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }
        // Если такого нет - добавляем в бд
        if (oldGenre == null)
        {
            // Создали новый экземпляр создаваемого объекта
            var newGenre = new Genre
            {
                GenreName = request.Name
            };
            await _applicationContext.Genres.AddAsync(newGenre, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }

        // Иначе юзеру придёт ошибка
        throw new NotAllowedException("Жанр с выбранным названием уже существует!");
    }
}