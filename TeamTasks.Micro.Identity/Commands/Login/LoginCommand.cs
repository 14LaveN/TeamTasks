using TeamTasks.Micro.Identity.Models.Identity;
using MediatR;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Micro.Identity.Commands.Login;

public class LoginCommand
    : IRequest<LoginResponse<User>>
{
    public required string UserName { get; set; } = null!;
    public required string Password { get; set; } = null!;
}