using FluentValidation;

namespace TeamTasks.Micro.TasksAPI.Commands.DoneTask;

public class DoneTaskCommandValidator
    : AbstractValidator<DoneTaskCommand>
{
    public DoneTaskCommandValidator()
    {
        RuleFor(x =>
                x.TaskId).NotEqual(Guid.Empty)
            .WithMessage("You don't choose the task.");
    }
}