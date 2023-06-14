using System.Net;

namespace BuildingBlocks.Core.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException(string message, int? code = null)
            : base(message, code: code, statusCode: HttpStatusCode.Conflict)
        {
        }
    }
}
