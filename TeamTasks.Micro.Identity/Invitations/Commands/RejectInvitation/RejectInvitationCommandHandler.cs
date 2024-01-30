using TeamTasks.Application.Core.Abstractions.Common;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Invitation.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Micro.Identity.Invitations.Commands.RejectInvitation;

/// <summary>
/// Represents the <see cref="RejectInvitationCommand"/> class.
/// </summary>
internal sealed class RejectInvitationCommandHandler : ICommandHandler<RejectInvitationCommand, Result>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork<Invitation> _unitOfWork;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="RejectInvitationCommandHandler"/> class.
    /// </summary>
    /// <param name="invitationRepository">The invitation repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    public RejectInvitationCommandHandler(
        IInvitationRepository invitationRepository,
        IUnitOfWork<Invitation> unitOfWork,
        IDateTime dateTime)
    {
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
    {
        Maybe<Invitation> maybeInvitation = await _invitationRepository.GetByIdAsync(request.InvitationId);

        if (maybeInvitation.HasNoValue)
        {
            return Result.Failure(DomainErrors.Invitation.NotFound);
        }

        Invitation invitation = maybeInvitation.Value;

        if (invitation.UserId != request.UserId)
        {
            return Result.Failure(DomainErrors.User.InvalidPermissions);
        }

        Result rejectResult = invitation.Reject(_dateTime.UtcNow);

        if (rejectResult.IsFailure)
        {
            return Result.Failure(rejectResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}