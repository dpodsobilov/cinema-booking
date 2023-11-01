using Data;
using Data.Models;
using Logic.DTO;
using Logic.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logic.Commands;

public class RegisterCommand : IRequest
{
    public string Email { get; }
    public string Password { get; }
    public string Name { get; }
    public string Surname { get; }
    public RegisterCommand(RegisterDto registerDto)
    {
        Email = registerDto.Email;
        Password = registerDto.Password;
        Name = registerDto.Name;
        Surname = registerDto.Surname;
    }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
    private readonly ApplicationContext _applicationContext;
    private readonly IMediator _mediator;

    public RegisterCommandHandler(ApplicationContext applicationContext, IMediator mediator)
    {
        _applicationContext = applicationContext;
        _mediator = mediator;
    }
    
    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = _applicationContext.Users.Any(u => u.Email == request.Email);

        if (email)
        {
            throw new Exception("Такой логин уже зарегистрирован");
        }
        
        await _applicationContext.Users.AddAsync(new User
        {
            Email = request.Email,
            Password = request.Password,
            Name = request.Name,
            Surname = request.Surname
        }, cancellationToken);
    }
}