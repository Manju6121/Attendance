using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AttendanceTracker.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(GlobalExceptionFilter));

        public void OnException(ExceptionContext context)
        {
            _logger.Error("Unhandled exception occurred", context.Exception);

            var response = new
            {
                Message = "Something went wrong.",
                Error = context.Exception.Message
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}