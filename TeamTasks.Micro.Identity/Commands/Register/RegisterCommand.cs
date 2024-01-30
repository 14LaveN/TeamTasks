using TeamTasks.Micro.Identity.Models.Identity;
using MediatR;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Micro.Identity.Commands.Register;

public class RegisterCommand : IRequest<LoginResponse<User>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommand"/> class.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="email">The emailAddress.</param>
    /// <param name="password">The password.</param>
    /// <param name="userName">The user name.</param>
    public RegisterCommand(string firstName, string lastName, string email, string password, string userName)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        UserName = userName;
    }

    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string UserName { get; }

    /// <summary>
    /// Gets the first name.
    /// </summary>
    public string FirstName { get;  }

    /// <summary>
    /// Gets the last name.
    /// </summary>
    public string LastName { get; }

    /// <summary>
    /// Gets the emailAddress.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets the password.
    /// </summary>
    public string Password { get; }
}