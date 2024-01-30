using FluentValidation;
using TeamTasks.Application.Core.Errors;
using TeamTasks.Application.Core.Extensions;

namespace TeamTasks.Micro.Identity.Commands.ChangePassword;

/// <summary>
/// Represents the <see cref="Micro.Identity.Commands.ChangePassword.ChangePasswordCommand"/> validator.
/// </summary>
public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordCommandValidator"/> class.
    /// </summary>
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(ValidationErrors.ChangePassword.UserIdIsRequired);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(ValidationErrors.ChangePassword.PasswordIsRequired);
    }
}