using Data;
using Data.Models;
using Logic.DTO.Admin.ForCreating;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.CreateCommands;

public class CreateFilmCommand : IRequest
{
    public string FilmName { get; }
    public string Hours { get; }
    public string Minutes { get; }
    public decimal FilmCoefficient { get; }
    public string Description { get; }
    public int Year { get; }
    public string Poster { get; }
    public int[] Genres { get; }

    public CreateFilmCommand(CreationFilmDto creationFilmDto)
    {
        FilmName = creationFilmDto.FilmName;
        Hours = creationFilmDto.Hours;
        Minutes = creationFilmDto.Minutes;
        FilmCoefficient = creationFilmDto.FilmCoefficient;
        Description = creationFilmDto.Description;
        Year = creationFilmDto.Year;
        Poster = creationFilmDto.Poster;
        Genres = creationFilmDto.Genres;
    }
}

public class CreateFilmCommandHandler : IRequestHandler<CreateFilmCommand>
{
    private readonly ApplicationContext _applicationContext;

    public CreateFilmCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Handle(CreateFilmCommand request, CancellationToken cancellationToken)
    {
        // Проверим, существуют ли жанры фильма
        foreach (var genreId in request.Genres)
        {
            var checkGenre = await _applicationContext.Genres
                .Where(g => g.GenreId == genreId)
                .FirstOrDefaultAsync(cancellationToken);
            if (checkGenre == null)
            {
                throw new NotFoundException("Выбранного жанра не существует!");
            }
        }
        
        // Получаем длительность в формате бд: "n часов m минут"
        var duration = FilmManager.GetDuration(request.Hours, request.Minutes);

        // Пытаемся найти такой экзмепляр в бд
        var oldFilm = await _applicationContext.Films
            // Сравнение по названию фильма
            .Where(g => g.FilmName.ToLower().Equals(request.FilmName.ToLower()))
            // Сравнение по году выпуска
            .Where(g => g.Year == request.Year)
            .FirstOrDefaultAsync(cancellationToken);

        // Если такой есть и он удалён -> восстанавливаем и обновляем
        if (oldFilm != null && oldFilm.IsDeleted)
        {
            oldFilm.IsDeleted = false;
            oldFilm.Description = request.Description;
            oldFilm.FilmCoefficient = request.FilmCoefficient;
            oldFilm.Duration = duration;
            oldFilm.Poster = Convert.FromBase64String(request.Poster);
            
            // Удалим старые жанры
            var oldFilmGenres = await _applicationContext.FilmGenres
                .Where(fg => fg.FilmId == oldFilm.FilmId)
                .ToListAsync(cancellationToken);
            
            _applicationContext.FilmGenres.RemoveRange(oldFilmGenres);
            
            // Добавим новые
            foreach (var genreId in request.Genres)
            {
                await _applicationContext.FilmGenres.AddAsync(new FilmGenre
                {
                    FilmId = oldFilm.FilmId,
                    GenreId = genreId
                }, cancellationToken);
            }
            
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }
        // Если такого нет -> создаём
        if (oldFilm == null)
        {
            // сначала добавляем фильм
            var newFilm = new Film
            {
                FilmName = request.FilmName,
                Duration = duration,
                FilmCoefficient = request.FilmCoefficient,
                Description = request.Description,
                Year = request.Year,
                IsDeleted = false,
                Poster = Convert.FromBase64String(request.Poster)
            };
            await _applicationContext.Films.AddAsync(newFilm, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            
            // затем добавляем связи фильм-жанр
            foreach (var genreId in request.Genres)
            {
                await _applicationContext.FilmGenres.AddAsync(new FilmGenre
                {
                    FilmId = newFilm.FilmId,
                    GenreId = genreId
                }, cancellationToken);
            }
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return;
        }

        // Иначе юзеру придёт ошибка
        throw new NotAllowedException("Фильм с таким названием и годом выпуска уже существует!");
    }
}