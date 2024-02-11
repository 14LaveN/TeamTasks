using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Enumerations;

namespace TeamTasks.Application.ApiHelpers.Responses;

public class BaseResponse<T> : IBaseResponse<T>
    where T : Result
{
    public required string Description { get; set; }

    public Task<T> Data { get; set; }

    public required StatusCode StatusCode { get; set; }
}