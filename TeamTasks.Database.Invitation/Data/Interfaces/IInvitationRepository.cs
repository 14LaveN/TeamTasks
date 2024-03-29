﻿using TeamTasks.Domain.Core.Errors;
using TeamTasks.Domain.Core.Primitives.Maybe;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Database.Invitation.Data.Interfaces;

/// <summary>
/// Represents the invitation repository interface.
/// </summary>
public interface IInvitationRepository
{
    /// <summary>
    /// Gets the invitation with the specified identifier.
    /// </summary>
    /// <param name="invitationId">The invitation identifier.</param>
    /// <returns>The maybe instance that may contain the invitation with the specified identifier.</returns>
    Task<Maybe<Domain.Entities.Invitation>> GetByIdAsync(Guid invitationId);

    /// <summary>
    /// Checks if an invitation for the specified event has already been sent.
    /// </summary>
    /// <param name="groupEvent">The event.</param>
    /// <param name="user">The user.</param>
    /// <returns></returns>
    Task<bool> CheckIfInvitationAlreadySentAsync(Domain.Entities.GroupEvent groupEvent, User user);

    /// <summary>
    /// Inserts the specified invitation to the database.
    /// </summary>
    /// <param name="invitation">The invitation to be inserted to the database.</param>
    Task Insert(Domain.Entities.Invitation invitation);

    /// <summary>
    /// Removes all of the invitations for the specified group event.
    /// </summary>
    /// <param name="groupEvent">The group event.</param>
    /// <param name="utcNow">The current date and time in UTC format.</param>
    /// <returns>The completed task.</returns>
    Task RemoveInvitationsForGroupEventAsync(Domain.Entities.GroupEvent groupEvent, DateTime utcNow);
}