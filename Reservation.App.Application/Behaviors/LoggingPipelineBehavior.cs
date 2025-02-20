using MediatR;
using Microsoft.Extensions.Logging;

namespace Reservation.App.Application.Behaviors
{
    public class LoggingPipelineBehavior<TRequest, TResponse>(
        ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger
    ) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Starting request {@RequestName}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                DateTime.UtcNow
            );

            try
            {
                var result = await next();

                _logger.LogInformation(
                    "Completed request {@RequestName}, {@DateTimeUtc}",
                    typeof(TRequest).Name,
                    DateTime.UtcNow
                );

                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Request failure {@RequestName}, {@ExceptionMessage}, {@ExceptionStackTrace}, {@InnerException}, {@DateTimeUtc}",
                    typeof(TRequest).Name,
                    exception.Message,
                    exception.StackTrace,
                    exception.InnerException,
                    DateTime.UtcNow
                );

                _logger.LogInformation(
                    "Completed request {@RequestName}, {@DateTimeUtc}",
                    typeof(TRequest).Name,
                    DateTime.UtcNow
                );

                throw;
            }
        }
    }
}
