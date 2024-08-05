using System.Net;

namespace Argo.MD.BuildingBlocks.Core.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message, int? code = null)
            : base(message, code: code, statusCode: HttpStatusCode.NotFound)
        {
        }
    }
}
