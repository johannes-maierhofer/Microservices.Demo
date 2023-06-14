using System.Net;

namespace BuildingBlocks.Core.Exceptions;

public class AppException : CustomException
{
    public AppException(string message, int? code = null) : base(message, code: code)
    {
    }

    public AppException()
    {
    }

    public AppException(string message, HttpStatusCode statusCode, int? code = null) : base(message, statusCode, code)
    {
    }

    public AppException(string message, Exception innerException, int? code = null) : base(message, innerException, code: code)
    {
    }
}
