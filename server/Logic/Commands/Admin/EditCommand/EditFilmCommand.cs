using Data;
using Data.Models;
using Logic.DTO.Admin.ForEditing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin.EditCommand;

/// <summary>
/// Редактирование фильма.
/// </summary>
public class EditFilmCommand : IRequest
{
    public int FilmId { get; }
    public string FilmName { get; }
    public string Hours { get; }
    public string Minutes { get; }
    public decimal FilmCoefficient { get; }
    public string Description { get; }
    public int Year { get; }
    public string Poster { get; }
    public int[] Genres { get; }

    public EditFilmCommand(EditFilmDto editFilmDto)
    {
        FilmId = editFilmDto.FilmId;
        FilmName = editFilmDto.FilmName;
        Hours = editFilmDto.Hours;
        Minutes = editFilmDto.Minutes;
        FilmCoefficient = editFilmDto.FilmCoefficient;
        Description = editFilmDto.Description;
        Year = editFilmDto.Year;
        Poster = editFilmDto.Poster;
        Genres = editFilmDto.Genres;
    }
}

public class EditFilmCommandHandler : IRequestHandler<EditFilmCommand>
{
    private readonly ApplicationContext _applicationContext;

    public EditFilmCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Handle(EditFilmCommand request, CancellationToken cancellationToken)
    {
        // Проверяем, существует ли выбранный фильм
        var film = await _applicationContext.Films
            .Where(f => f.FilmId == request.FilmId)
            .Where(f => f.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);

        if (film == null)
        {
            throw new Exception("Выбранный фильм не существует!");
        }

        // Проверяем, не используется ли фильм в расписании
        var sessions = await _applicationContext.Sessions
            .Where(s => s.IsDeleted == false)
            .Where(s => s.FilmId == request.FilmId)
            .ToListAsync(cancellationToken);

        if (sessions.Count != 0)
        {
            throw new Exception("Выбранный фильм уже используется в расписании!");
        }
        
        // Проверка пришедших жанров опущена - предполагаем, что придут "хорошие"

        // Проверяем, нет ли другого фильма с выбранным названием и годом выпуска
        var otherFilm = await _applicationContext.Films
            .Where(f => f.FilmId != request.FilmId)
            .Where(f => f.IsDeleted == false)
            .Where(f => f.FilmName.ToLower().Equals(request.FilmName.ToLower()))
            .Where(f => f.Year == request.Year)
            .FirstOrDefaultAsync(cancellationToken);

        if (otherFilm != null)
        {
            throw new Exception("Фильм с таким названием и годом выпуска уже существует!");
        }

        // Если все проверки пройдены -> редактируем фильм
        film.FilmName = request.FilmName;
        film.Duration = FilmManager.GetDuration(request.Hours, request.Minutes);
        film.FilmCoefficient = request.FilmCoefficient;
        film.Description = request.Description;
        film.Year = request.Year;
        film.Poster = Convert.FromBase64String(request.Poster);

        // Удаляем старые жанры фильма
        var oldFilmGenres = await _applicationContext.FilmGenres
            .Where(fg => fg.FilmId == request.FilmId)
            .ToListAsync(cancellationToken);
        _applicationContext.FilmGenres.RemoveRange(oldFilmGenres);

        // Добавляем новые
        var newFilmGenres = request.Genres.Select(requestGenre => 
            new FilmGenre
            {
                FilmId = request.FilmId, 
                GenreId = requestGenre
            }).ToList();
        await _applicationContext.FilmGenres.AddRangeAsync(newFilmGenres, cancellationToken);

        // Сохраняем изменения
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}