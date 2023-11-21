using Logic.DTO;
using Logic.DTO.Admin;
using Logic.Queries;
using Logic.Queries.Admin;
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
    
}