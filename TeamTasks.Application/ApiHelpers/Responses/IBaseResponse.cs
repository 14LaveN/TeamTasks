using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Enumerations;

namespace TeamTasks.Application.ApiHelpers.Responses;

public interface IBaseResponse<T>
{
    public StatusCode StatusCode { get; set; }

    public string Description { get; set; }
    
    public Task<Result> Data { get; set; }
}