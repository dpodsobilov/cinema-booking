using Logic.DTO;
using Logic.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
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
}