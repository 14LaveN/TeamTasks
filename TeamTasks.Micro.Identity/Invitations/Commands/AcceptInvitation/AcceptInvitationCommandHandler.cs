using MediatR;
using TeamTasks.Application.Core.Abstractions.Common;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Invitation.Data.Interfaces;
using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Micro.Identity.Invitations.Commands.AcceptInvitation;

/// <summary>
/// Represents the <see cref="AcceptInvitationCommand"/> class.
/// </summary>
internal sealed class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand, Result>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork<Invitation> _unitOfWork;
    private readonly IDateTime _dateTime;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AcceptInvitationCommandHandler"/> class.
    /// </summary>
    /// <param name="invitationRepository">The invitation repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTime">The date and time.</param>
    /// <param name="mediator">The mediator.</param>
    public AcceptInvitationCommandHandler(
        IInvitationRepository invitationRepository,
        IUnitOfWork<Invitation> unitOfWork,
        IDateTime dateTime,
        IMediator mediator)
    {
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
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

        Result result = invitation.Accept(_dateTime.UtcNow);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Result.Success();
    }
}