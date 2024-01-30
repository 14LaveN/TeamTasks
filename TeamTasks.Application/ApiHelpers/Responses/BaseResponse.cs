using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Enumerations;

namespace TeamTasks.Application.ApiHelpers.Responses;

public class BaseResponse<T> : IBaseResponse<T>
    where T : class
{
    public required string Description { get; set; }

    public Task<Result> Data { get; set; }

    public required StatusCode StatusCode { get; set; }
}