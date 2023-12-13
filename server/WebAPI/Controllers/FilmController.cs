using Logic.DTO;
using Logic.DTO.User;
using Logic.Queries;
using Logic.Queries.User;
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