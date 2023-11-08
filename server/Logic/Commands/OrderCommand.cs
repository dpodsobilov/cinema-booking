using Data;
using Data.Models;
using Logic.DTO;
using MediatR;

namespace Logic.Commands;

public class OrderCommand : IRequest
{
    public int UserId { get; }
    public int SessionId { get; }
    public ICollection<PlaceAndCostDTO> PlacesAndCost { get; }

    public OrderCommand(OrderDTO orderDto)
    {
        UserId = orderDto.UserId;
        SessionId = orderDto.SessionId;
        PlacesAndCost = orderDto.PlaceAndCost;
    }
}

public class OrderCommandHandler : IRequestHandler<OrderCommand>
{
    private readonly ApplicationContext _applicationContext;

    public OrderCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(OrderCommand request, CancellationToken cancellationToken)
    {
        foreach (PlaceAndCostDTO element in request.PlacesAndCost)
        {
            var check = _applicationContext.Tickets.Any(t =>
                (t.SessionId == request.SessionId) && (t.PlaceId == element.PlaceId));
            if (check)
            {
                throw new Exception("Такой билет уже есть!");
            }
            
            await _applicationContext.Tickets.AddAsync(new Ticket()
            {
                UserId = request.UserId,
                SessionId = request.SessionId,
                PlaceId = element.PlaceId ,
                Price = element.Price
            }, cancellationToken);
        }
        await _applicationContext.SaveChangesAsync(cancellationToken);   
    }
}