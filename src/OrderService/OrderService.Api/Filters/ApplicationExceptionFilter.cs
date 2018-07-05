using CorrelationId;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace OrderService.Api.Filters
{
    public class ApplicationExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ICorrelationContextAccessor _correlationContext;
        private readonly ILogger _logger;

        public ApplicationExceptionFilter(
            ICorrelationContextAccessor correlationContext,
            ILoggerFactory loggerFactory)
        {
            _correlationContext = correlationContext;
            _logger = loggerFactory.CreateLogger(typeof(ApplicationExceptionFilter).Name);
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(
                $"Request with CorrelationId {_correlationContext.CorrelationContext.CorrelationId} throwed {context.Exception}");
            
            context.Result = new JsonResult(new {message = context.Exception.Message});
        }
    }
}