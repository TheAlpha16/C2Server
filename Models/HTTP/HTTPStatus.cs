namespace C2Server.Models.HTTP;

public enum HTTPStatus
{
    OK = 200,
    Created = 201,
    Accepted = 202,
    NoContent = 204,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InternalServerError = 500,
    NotImplemented = 501,
    ServiceUnavailable = 503
}

public static class HttpStatusExtensions
{
    public static string Message(this HTTPStatus status)
    {
        return status switch
        {
            HTTPStatus.OK => "OK",
            HTTPStatus.Created => "Created",
            HTTPStatus.Accepted => "Accepted",
            HTTPStatus.NoContent => "No Content",
            HTTPStatus.BadRequest => "Bad Request",
            HTTPStatus.Unauthorized => "Unauthorized",
            HTTPStatus.Forbidden => "Forbidden",
            HTTPStatus.NotFound => "Not Found",
            HTTPStatus.InternalServerError => "Internal Server Error",
            HTTPStatus.NotImplemented => "Not Implemented",
            HTTPStatus.ServiceUnavailable => "Service Unavailable",
            _ => "Unknown Status"
        };
    }
}