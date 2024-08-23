using GitIssuer.Domain;
using MediatR;

namespace GitIssuer.Application.Repository.Commands.UpdateIssue;

public record UpdateIssueCommand(
    ContentType ContentType,
    string Owner,
    string Repository,
    long IssueId,
    string? State,
    string? StateReason,
    string? Title,
    string? Body,
    List<string>? Assignees,
    List<string>? Milestones,
    List<string>? Labels
) : IRequest<Unit>;