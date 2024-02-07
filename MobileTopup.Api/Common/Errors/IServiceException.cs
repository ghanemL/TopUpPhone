using System.Net;

namespace MobileTopup.Api.Common.Errors
{
    public interface IServiceException
    {
        public HttpStatusCode StatusCode { get; }

        public string ErrorMessage { get; }
    }
}
