using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands.Admin;

public class DeleteTemplateCommand : IRequest
{
    public int TemplateId { get; }

    public DeleteTemplateCommand(int templateId)
    {
        TemplateId = templateId;
    }
}

public class DeleteTemplateCommandHandler : IRequestHandler<DeleteTemplateCommand>
{
    private readonly ApplicationContext _applicationContext;

    public DeleteTemplateCommandHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
    {
        var hallId = new List<int>();
        
        var template = await _applicationContext.CinemaHallTypes
            .Where(template => template.CinemaHallTypeId == request.TemplateId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (template != null)
        {
            template.IsDeleted = true;
            
            var halls = await _applicationContext.CinemaHalls
                .Where(hall => hall.CinemaHallTypeId == request.TemplateId)
                .Select(hall => hall)
                .ToListAsync(cancellationToken);

            if (halls.Count != 0)
            {
                foreach (var hall in halls)
                {
                    hall.IsDeleted = true;

                    hallId.Add(hall.CinemaHallId);
                }

                var sessions = await _applicationContext.Sessions
                    .Where(session => hallId.Contains(session.CinemaHallId))
                    .Select(session => session)
                    .ToListAsync(cancellationToken);

                if (sessions.Count != 0)
                {
                    foreach (var session in sessions)
                    {
                        session.IsDeleted = true;
                    }
                }
                else throw new Exception("Ошибка!");
            }
            else throw new Exception("Ошибка!");
            
            await _applicationContext.SaveChangesAsync(cancellationToken);
        }
        else throw new Exception("Ошибка!");
    }
}