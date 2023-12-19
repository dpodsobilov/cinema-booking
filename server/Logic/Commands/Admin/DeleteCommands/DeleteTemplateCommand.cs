using Data;
using Logic.Exceptions;
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
            .Where(template => template.CinemaHallTypeId == request.TemplateId
                    && template.IsDeleted == false)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (template == null)
        {
            throw new NotFoundException("Выбранный шаблон не существует!");
        }

        var halls = await _applicationContext.CinemaHalls
            .Where(hall => hall.CinemaHallTypeId == template.CinemaHallTypeId
                    && hall.IsDeleted == false)
            .Select(hall => hall)
            .ToListAsync(cancellationToken);
        
        if (halls.Count == 0)
        {
            template.IsDeleted = true;
            await _applicationContext.SaveChangesAsync(cancellationToken);
        }
        
        else
        {
            int counter = 0;
            foreach (var hall in halls)
            {
                var sessions = await _applicationContext.Sessions
                    .Where(session => session.CinemaHallId == hall.CinemaHallId && session.IsDeleted == false
                        && session.DataTimeSession > DateTime.Now)
                    .Select(session => session)
                    .ToListAsync(cancellationToken);

                if (sessions.Count == 0) { counter++; }
            }

            if (counter == halls.Count)
            {
                template.IsDeleted = true;
                foreach (var hall in halls)
                {
                    hall.IsDeleted = true;
                    
                    var sessionsDelete = await _applicationContext.Sessions
                        .Where(ses => ses.CinemaHallId == hall.CinemaHallId && ses.IsDeleted == false)
                        .Select(ses => ses)
                        .ToListAsync(cancellationToken);

                    if (sessionsDelete.Count != 0)
                    {
                        foreach (var sd in sessionsDelete)
                        {
                            sd.IsDeleted = true;
                        }
                    }
                }
                await _applicationContext.SaveChangesAsync(cancellationToken);
            }
            else throw new NotAllowedException("Шаблон используется!");
        }
    }
}