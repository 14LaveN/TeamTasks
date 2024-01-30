using FluentValidation;
using TeamTasks.Application.Core.Errors;
using TeamTasks.Application.Core.Extensions;

namespace TeamTasks.Micro.Identity.Invitations.Commands.RejectInvitation;

/// <summary>
/// Represents the <see cref="RejectInvitationCommand"/> validator.
/// </summary>
public sealed class RejectInvitationCommandValidator : AbstractValidator<RejectInvitationCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RejectInvitationCommandValidator"/> class.
    /// </summary>
    public RejectInvitationCommandValidator() =>
        RuleFor(x => x.InvitationId)
            .NotEmpty()
            .WithError(ValidationErrors.RejectInvitation.InvitationIdIsRequired);
}