using Logic.Commands;
using Logic.DTO;
using Logic.DTO.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddTicket(OrderDto orderDto)
    {
        await _mediator.Send(new OrderCommand(orderDto));
        return Ok();
    }
}