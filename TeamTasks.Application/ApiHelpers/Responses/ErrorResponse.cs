namespace TeamTasks.Application.ApiHelpers.Responses;

public sealed class ErrorResponse
{
    public required string Message { get; set; }
    
    public required string ErrorCode { get; set; }
}