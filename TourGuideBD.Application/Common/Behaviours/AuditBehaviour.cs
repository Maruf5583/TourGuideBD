using System.Text.Json;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Audit;

namespace TourGuideBD.Application.Common.Behaviours;

public interface IAuditableRequest
{
    string ActionName { get; }
    string EntityName { get; }
    string? EntityId { get; }
}

public class AuditBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AuditBehaviour(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is IAuditableRequest auditable)
        {
            var log = new AuditLog
            {
                UserId = _currentUserService.UserId,
                UserName = _currentUserService.UserName ?? "system",
                Action = auditable.ActionName,
                EntityName = auditable.EntityName,
                EntityId = auditable.EntityId,
                NewValues = JsonSerializer.Serialize(request),
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return response;
    }
}