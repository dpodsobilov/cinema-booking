using Data;
using Data.Models;
using Logic.DTO;
using Logic.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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
    private readonly IHubContext<PlacesHub> _hubContext;

    public OrderCommandHandler(ApplicationContext applicationContext, IHubContext<PlacesHub> hubContext)
    {
        _applicationContext = applicationContext;
        _hubContext = hubContext;
    }
    
    public async Task Handle(OrderCommand request, CancellationToken cancellationToken)
    {
        await CreateOrder(request, cancellationToken);

        var places = await GetPlacesInOrder(request, cancellationToken);

        await _hubContext.Clients.All.SendAsync("updatePlaces", places, cancellationToken);
    }
    
    /// <summary>
    /// Создание билетов в бд
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="Exception"></exception>
    private async Task CreateOrder(OrderCommand request, CancellationToken cancellationToken)
    {
        foreach (PlaceAndCostDTO element in request.PlacesAndCost)
        {
            var check = _applicationContext.Tickets.Any(t =>
                (t.SessionId == request.SessionId) && (t.PlaceId == element.PlaceId));
            if (check)
            {
                throw new Exception("Такой билет уже есть!");
            }

            var check2 = await _applicationContext.Sessions
                .Where(session => session.SessionId == request.SessionId && session.IsDeleted == true)
                .FirstOrDefaultAsync(cancellationToken);
            if (check2 != null)
            {
                throw new Exception("Такого сеанса уже нет!");
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

    /// <summary>
    /// Получение id мест по созданным билетам
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<IList<int>> GetPlacesInOrder(OrderCommand request, CancellationToken cancellationToken)
    {
        var result = new List<int>();
        
        foreach (var element in request.PlacesAndCost)
        {
            var ticket = await _applicationContext.Tickets
                .Where(t => t.SessionId == request.SessionId && t.PlaceId == element.PlaceId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (ticket != null)
            {
                result.Add(ticket.PlaceId);
            }
        }

        return result;
    }
}