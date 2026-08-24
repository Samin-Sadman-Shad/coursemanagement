using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            using (_logger.BeginScope
                    (new Dictionary<string, object>
                        {
                            ["RequestId"] = Guid.NewGuid(),
                            ["RequestName"] = requestName
                        }
                    )
                )
            {
                _logger.LogInformation("Handling {RequestName} {@Request}", requestName, request);
                var sw = Stopwatch.StartNew();

                try
                {
                    var response = await next();
                    _logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds} ms",
                        requestName, sw.ElapsedMilliseconds);
                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling {RequestName} after {ElapsedMilliseconds} ms",
                        requestName, sw.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
