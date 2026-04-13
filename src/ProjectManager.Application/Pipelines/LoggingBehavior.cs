using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Pipelines
{
    public class LoggingBehavior <TRequest, TResponse> :IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            // Log antes de ejecutar el Handler
            _logger.LogInformation("ProjectManager Request: {Name} {@Request}",
                requestName, request);

            var response = await next();

            // Log después de ejecutar el Handler
            _logger.LogInformation("ProjectManager Completed: {Name}", requestName);

            return response;
        }
    }
}
