using System.Globalization;
using System.Net;

namespace BuildingBlocks.Core.Exceptions
{
    public class InternalServerException : CustomException
    {
        public InternalServerException()
        { }

        public InternalServerException(string message, int? code) : base(message, HttpStatusCode.InternalServerError, code: code) { }

        public InternalServerException(string message, int? code = null, params object[] args)
            : base(message: string.Format(CultureInfo.CurrentCulture, message, args, HttpStatusCode.InternalServerError, code))
        {
        }
    }
}
