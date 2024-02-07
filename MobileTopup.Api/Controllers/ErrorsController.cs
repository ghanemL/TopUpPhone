using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MobileTopup.Api.Common.Errors;

namespace MobileTopup.Api.Controllers
{
    [Route("/error")]
    public class ErrorsController : ControllerBase
    {
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            var (statusCode, message) = exception switch
            {
                IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };

            return Problem(statusCode: statusCode, title: message);
        }
    }
}
