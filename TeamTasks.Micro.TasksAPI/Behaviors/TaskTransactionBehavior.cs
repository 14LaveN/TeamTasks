using MediatR;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Micro.TasksAPI.Behaviors;

/// <summary>
/// Represents the <see cref="AnswerEntity"/> transaction behaviour class.
/// </summary>
internal sealed class TaskTransactionBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    private readonly IUnitOfWork<TaskEntity> _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskTransactionBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public TaskTransactionBehavior(IUnitOfWork<TaskEntity> unitOfWork) =>
        _unitOfWork = unitOfWork;

    /// <inheritdoc/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is IQuery<TResponse>)
        {
            return await next();
        }

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            TResponse response = await next();

            await transaction.CommitAsync(cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }
    }
}