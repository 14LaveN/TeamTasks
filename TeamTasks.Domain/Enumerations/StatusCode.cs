namespace TeamTasks.Domain.Enumerations;

public enum StatusCode
{
    TaskIsHasAlready = 1,
    Ok = 200,
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    InternalServerError = 500
}