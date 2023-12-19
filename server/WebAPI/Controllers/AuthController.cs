using Logic.Commands;
using Logic.DTO;
using Logic.DTO.User;
using Logic.Queries;
using Logic.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
    {
        var user = await _mediator.Send(new LoginQuery(loginDto));
        return Ok(user);
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto loginDto)
    {
        await _mediator.Send(new RegisterCommand(loginDto));
        return Ok();
    }
    
}