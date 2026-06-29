using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? "anonymous";

        _logger.LogInformation("TourGuideBD Request: {Name} {@UserId}", requestName, userId);

        return Task.CompletedTask;
    }
}