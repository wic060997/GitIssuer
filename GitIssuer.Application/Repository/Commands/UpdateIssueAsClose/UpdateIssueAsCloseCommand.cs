using GitIssuer.Domain;
using MediatR;

namespace GitIssuer.Application.Repository.Commands.UpdateIssueAsClose;

public record UpdateIssueAsCloseCommand(
    ContentType ContentType,
    string Owner,
    string Repository,
    long IssueId)
    : IRequest<Unit>;