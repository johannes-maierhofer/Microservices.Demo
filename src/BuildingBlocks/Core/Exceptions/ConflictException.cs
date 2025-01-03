using System.Net;

namespace Argo.MD.BuildingBlocks.Core.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException(string message, int? code = null)
            : base(message, code: code, statusCode: HttpStatusCode.Conflict)
        {
        }
    }
}
