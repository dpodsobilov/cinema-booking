using Data;
using Logic.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin;

public class DeleteSessionCommand : IRequest
{
    public int SessionId { get; }

    public DeleteSessionCommand(int sessionId)
    {
        SessionId = sessionId;
    }
}

public class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand>
{
    private readonly ApplicationContext _applicationContext;

    public DeleteSessionCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        //находим сеанс
        var session = await _applicationContext.Sessions
            .Where(session => session.SessionId == request.SessionId
                    && session.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (session == null)
        {
            throw new NotFoundException("Выбранный сеанс не существует!");
        }

        //находим билеты на сеанс
        var ticket = await _applicationContext.Tickets
            .Where(ticket => ticket.SessionId == session.SessionId
                    && session.DataTimeSession > DateTime.Now)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (ticket != null)
        {
         throw new NotAllowedException("На выбранный сеанс проданы билеты!");
        }

        session.IsDeleted = true;
        await _applicationContext.SaveChangesAsync(cancellationToken);
    }
}