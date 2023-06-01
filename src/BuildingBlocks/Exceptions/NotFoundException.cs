using System.Net;

namespace BuildingBlocks.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message, int? code = null) 
            : base(message, code: code, statusCode: HttpStatusCode.NotFound)
        {
        }
    }
}