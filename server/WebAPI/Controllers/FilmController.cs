using Logic.DTO;
using Logic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmController : ControllerBase
{
    private readonly IMediator _mediator;

    public FilmController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // [HttpPost]
    // public async Task<FilmDto> GetFilmInfo([FromBody] int filmId)
    // {
    //     var film = await _mediator.Send(new GetFilmQuery(filmId));
    //     return film;
    // }
    
    [HttpGet]
    public async Task<FilmDto> GetFilmInfo(int param)
    {
        var film = await _mediator.Send(new GetFilmQuery(param));
        return film;
    }
    
    [HttpPost]
    public async Task<IList<FilmScheduleDto>> GetSchedule([FromBody] int filmId)
    {
        var schedule = await _mediator.Send(new GetScheduleQuery(filmId));
        return schedule;
    }
}