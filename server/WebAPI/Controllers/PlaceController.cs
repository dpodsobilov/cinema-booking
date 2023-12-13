using Logic.DTO;
using Logic.DTO.User;
using Logic.Queries;
using Logic.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaceController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlaceController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<CinemaHallDto> GetPlaces(int sessionId)
    {
        var cinemaHallDto = await _mediator.Send(new GetCinemaHallQuery(sessionId));
        return cinemaHallDto;
    }
}