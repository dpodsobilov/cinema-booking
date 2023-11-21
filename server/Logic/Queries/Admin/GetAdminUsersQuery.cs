using Data;
using Logic.DTO.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries.Admin;

public class GetAdminUsersQuery : IRequest<IList<AdminUserDto>>
{ }

public class GetAdminUsersQueryHandler : IRequestHandler<GetAdminUsersQuery, IList<AdminUserDto>>
{
    private readonly ApplicationContext _applicationContext;

    public GetAdminUsersQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<IList<AdminUserDto>> Handle(GetAdminUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _applicationContext.Users.Select(user => new AdminUserDto()
        {
            Email = user.Email,
            Name = user.Name,
            Surname = user.Surname
        }).ToListAsync(cancellationToken);
        
        return users;
    }
}