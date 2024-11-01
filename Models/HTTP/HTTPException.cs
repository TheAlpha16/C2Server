namespace C2Server.Models.HTTP;

public class HTTPException : Exception
{
    public HTTPStatus StatusCode { get; }

    public HTTPException(HTTPStatus statusCode, string? message = null)
        : base(message ?? statusCode.Message())
    {
        StatusCode = statusCode;
    }
}

public class BadRequestException : HTTPException
{
    public BadRequestException(string? message = null)
        : base(HTTPStatus.BadRequest, message) { }
}

public class UnauthorizedException : HTTPException
{
    public UnauthorizedException(string? message = null)
        : base(HTTPStatus.Unauthorized, message) { }
}

public class NotFoundException : HTTPException
{
    public NotFoundException(string? message = null)
        : base(HTTPStatus.NotFound, message) { }
}

public class InternalServerErrorException : HTTPException
{
    public InternalServerErrorException(string? message = null)
        : base(HTTPStatus.InternalServerError, message) { }
}
