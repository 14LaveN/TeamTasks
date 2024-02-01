using FluentValidation;

namespace TeamTasks.Micro.TasksAPI.Commands.CreateTask;

public sealed class CreateTaskCommandValidator
    : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x =>
                x.Description).NotEqual(string.Empty)
            .WithMessage("You didn't write a description.")
            .MaximumLength(512)
            .WithMessage("Your description too big.");
        
        RuleFor(x =>
                x.Title.Value).NotEqual(string.Empty)
            .WithMessage("You didn't write a title.")
            .MaximumLength(128)
            .WithMessage("Your title too big.");
    }
}