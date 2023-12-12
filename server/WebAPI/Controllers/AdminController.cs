using Logic.Commands.Admin;
using Logic.Commands.Admin.CreateCommands;
using Logic.Commands.Admin.EditCommand;
using Logic.DTO;
using Logic.DTO.Admin;
using Logic.DTO.Admin.ForCreating;
using Logic.DTO.Admin.ForEditing;
using Logic.Queries;
using Logic.Queries.Admin;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Get

    [HttpGet("Films")]
    public async Task<IList<AdminFilmDto>> GetFilmsInfo()
    {
        var films = await _mediator.Send(new GetAdminFilmQuery());
        return films;
    }
    
    [HttpGet("Genres")]
    public async Task<IList<AdminGenreDto>> GetGenresInfo()
    {
        var genres = await _mediator.Send(new GetAdminGenreQuery());
        return genres;
    }
    
    [HttpGet("Cinemas")]
    public async Task<IList<AdminCinemaDto>> GetCinemasInfo()
    {
        var cinemas = await _mediator.Send(new GetAdminCinemaQuery());
        return cinemas;
    }
    
    [HttpGet("Halls")]
    public async Task<IList<AdminHallDto>> GetHallsInfo(int param)
    {
        var halls = await _mediator.Send(new GetAdminHallQuery(param));
        return halls;
    }
    
    [HttpGet("Users")]
    public async Task<IList<AdminUserDto>> GetUsersInfo()
    {
        var users = await _mediator.Send(new GetAdminUsersQuery());
        return users;
    }
    
    [HttpGet("Sessions")]
    public async Task<IList<AdminSessionDto>> GetSessions()
    {
        var sessions = await _mediator.Send(new GetAdminSessionQuery());
        return sessions;
    }
    
    [HttpGet("Templates")]
    public async Task<IList<AdminTemplatesDto>> GetTemplates()
    {
        var templates = await _mediator.Send(new GetAdminTemplatesQuery());
        return templates;
    }
    
    [HttpGet("Template")]
    public async Task<AdminTemplateDto> GetTemplate(int param)
    {
        var template = await _mediator.Send(new GetAdminTemplateQuery(param));
        return template;
    }

    [HttpGet("Stats")]
    public async Task<IList<AdminStatDto>> GetStats()
    {
        var stats = await _mediator.Send(new GetAdminStatsQuery());
        return stats;
    }
    
    //для компоненты добавления шаблона
    [HttpGet("PlacesTypes")]
    public async Task<IList<AdminGetTemplatePlaceTypeDto>> GetPlacesTypes()
    {
        var placesTypes = await _mediator.Send(new GetAdminTemplatePlacesTypesQuery());
        return placesTypes;
    }
    
    //для таблицы с типами мест
    [HttpGet("PlaceType")]
    public async Task<IList<AdminGetPlaceTypeDto>> GetPlaceType()
    {
        var placeType = await _mediator.Send(new GetAdminPlaceTypeQuery());
        return placeType;
    }

    #endregion

    #region Delete

    [HttpDelete("Film")]
    public async Task<IActionResult> DeleteFilm(int filmId)
    {
        await _mediator.Send(new DeleteFilmCommand(filmId));
        return Ok();
    }
    
    [HttpDelete("Session")]
    public async Task<IActionResult> DeleteSession(int sessionId)
    {
        await _mediator.Send(new DeleteSessionCommand(sessionId));
        return Ok();
    }
    
    [HttpDelete("Genre")]
    public async Task<IActionResult> DeleteGenre(int genreId)
    {
        await _mediator.Send(new DeleteGenreCommand(genreId));
        return Ok();
    }
    
    [HttpDelete("Template")]
    public async Task<IActionResult> DeleteTemplate(int templateId)
    {
        await _mediator.Send(new DeleteTemplateCommand(templateId));
        return Ok();
    }
    
    [HttpDelete("Hall")]
    public async Task<IActionResult> DeleteHall(int cinemaHallId)
    {
        await _mediator.Send(new DeleteHallCommand(cinemaHallId));
        return Ok();
    }
    
    [HttpDelete("Cinema")]
    public async Task<IActionResult> DeleteCinema(int cinemaId)
    {
        await _mediator.Send(new DeleteCinemaCommand(cinemaId));
        return Ok();
    }
    
    [HttpDelete("PlaceType")]
    public async Task<IActionResult> CreatePlaceType(int placeTypeId)
    {
        await _mediator.Send(new DeletePlaceTypeCommand(placeTypeId));
        return Ok();
    }

    #endregion

    #region Post (Create)

    [HttpPost("Genre")]
    public async Task<IActionResult> CreateGenre(CreationGenreDto request)
    {
        await _mediator.Send(new CreateGenreCommand(request));
        return Ok();
    }
    
    [HttpPost("Film")]
    public async Task<IActionResult> CreateFilm(CreationFilmDto request)
    {
        await _mediator.Send(new CreateFilmCommand(request));
        return Ok();
    }
    
    [HttpPost("Cinema")]
    public async Task<IActionResult> CreateCinema(CreationCinemaDto request)
    {
        await _mediator.Send(new CreateCinemaCommand(request));
        return Ok();
    }
    
    [HttpPost("Hall")]
    public async Task<IActionResult> CreateCinemaHall(CreationCinemaHallDto request)
    {
        await _mediator.Send(new CreateCinemaHallCommand(request));
        return Ok();
    }
    
    [HttpPost("Template")]
    public async Task<IActionResult> CreateTemplate(CreationTemplateDto request)
    {
        await _mediator.Send(new CreateTemplateCommand(request));
        return Ok();
    }
    
    [HttpPost("PlaceType")]
    public async Task<IActionResult> CreatePlaceType(CreationPlaceTypeDto request)
    {
        await _mediator.Send(new CreatePlaceTypeCommand(request));
        return Ok();
    }

    #endregion

    #region Put (Edit)

    [HttpPut("Genre")]
    public async Task<IActionResult> EditGenre(AdminGenreDto request)
    {
        await _mediator.Send(new EditGenreCommand(request));
        return Ok();
    }
    
    [HttpPut("Hall")]
    public async Task<IActionResult> EditHall(EditHallDto request)
    {
        await _mediator.Send(new EditHallCommand(request));
        return Ok();
    }
    
    [HttpPut("Template")]
    public async Task<IActionResult> EditTemplate(EditTemplateDto request)
    {
        await _mediator.Send(new EditTemplateCommand(request));
        return Ok();
    }

    #endregion
    
}