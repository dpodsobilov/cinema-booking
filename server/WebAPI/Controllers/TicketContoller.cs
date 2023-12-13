using Logic.DTO;
using Logic.DTO.User;
using Logic.Queries;
using Logic.Queries.User;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<AllUsersTicketsDto> GetSchedule([FromBody] int userId)
    {
        var tickets = await _mediator.Send(new GetUserTicketsQuery(userId));
        return tickets;
    }
}