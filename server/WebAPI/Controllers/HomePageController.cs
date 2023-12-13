using Logic.DTO;
using Logic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HomePageController : ControllerBase
{
    private readonly IMediator _mediator;

    public HomePageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IList<HomePageDto>> Get()
    {
        var films = await _mediator.Send(new GetHomePageFilmsQuery());
        return films;
    }
}