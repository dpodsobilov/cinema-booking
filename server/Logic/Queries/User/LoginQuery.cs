using Data;
using Logic.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Queries;

public class LoginQuery : IRequest<UserDto>
{
    public string Email { get; }
    public string Password { get; }

    public LoginQuery(LoginDto loginDto)
    {
        Email = loginDto.Email;
        Password = loginDto.Password;
    }
}

public class LoginQueryHandler : IRequestHandler<LoginQuery, UserDto>
{
    private readonly ApplicationContext _applicationContext;

    public LoginQueryHandler(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<UserDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _applicationContext.Users
            .Where(u => u.Email == request.Email && u.Password == request.Password)
            .Select(u => new UserDto
            {
                UserId = u.UserId,
                Role = u.Role,
                Name = u.Name,
                Surname = u.Surname
            }).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new Exception("Не удалось авторизоваться!");
        }
        
        return user;
    }
}