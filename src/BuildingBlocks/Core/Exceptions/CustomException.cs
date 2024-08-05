using System.Net;

namespace Argo.MD.BuildingBlocks.Core.Exceptions;

public class CustomException : Exception
{
    public CustomException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        int? code = null) : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public CustomException(
        string message,
        Exception innerException,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        int? code = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public CustomException(
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        int? code = null)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public HttpStatusCode StatusCode { get; }

    public int? Code { get; }
}
