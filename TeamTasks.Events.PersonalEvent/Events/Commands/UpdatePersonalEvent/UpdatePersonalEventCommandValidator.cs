﻿using FluentValidation;
using TeamTasks.Application.Core.Errors;
using TeamTasks.Application.Core.Extensions;

namespace TeamTasks.Events.PersonalEvent.Events.Commands.UpdatePersonalEvent;

/// <summary>
/// Represents the <see cref="UpdatePersonalEventCommand"/> validator.
/// </summary>
public sealed class UpdatePersonalEventCommandValidator : AbstractValidator<UpdatePersonalEventCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref=""/>
    /// </summary>
    public UpdatePersonalEventCommandValidator()
    {
        RuleFor(x => x.PersonalEventId)
            .NotEmpty()
            .WithError(ValidationErrors.UpdatePersonalEvent.GroupEventIdIsRequired);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(ValidationErrors.UpdatePersonalEvent.NameIsRequired);

        RuleFor(x => x.DateTimeUtc)
            .NotEmpty()
            .WithError(ValidationErrors.UpdatePersonalEvent.DateAndTimeIsRequired);
    }
}