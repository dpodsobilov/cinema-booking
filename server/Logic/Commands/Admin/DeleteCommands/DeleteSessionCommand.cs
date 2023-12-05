using Data;
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
        var session = await _applicationContext.Sessions.Where(session => session.SessionId == request.SessionId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (session != null)
        {
            session.IsDeleted = true;
            
            await _applicationContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new Exception("Ошибка!");
        }
    }
}