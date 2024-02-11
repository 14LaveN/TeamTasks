using TeamTasks.Application.ApiHelpers.Responses;
using TeamTasks.Application.Core.Abstractions.Messaging;
using TeamTasks.Domain.Core.Primitives.Result;

namespace TeamTasks.Micro.Company.Commands.AddToCompany;

/// <summary>
/// Represents the add to company command record.
/// </summary>
public sealed record AddToCompanyCommand(
    Guid CompanyId,
    Guid UserId)
    : ICommand<IBaseResponse<Result>>;