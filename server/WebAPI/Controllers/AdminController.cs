﻿using Logic.Commands.Admin;
using Logic.DTO;
using Logic.DTO.Admin;
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

    [HttpDelete("Film")]
    public async Task<IActionResult> DeleteFilm(int filmId)
    {
        await _mediator.Send(new DeleteFilmCommand(filmId));
        return Ok();
    }
    
    [HttpGet("Sessions")]
    public async Task<IList<AdminSessionDto>> GetSessions()
    {
        var sessions = await _mediator.Send(new GetAdminSessionQuery());
        return sessions;
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
}