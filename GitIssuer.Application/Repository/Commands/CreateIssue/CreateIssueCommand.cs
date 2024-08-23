using GitIssuer.Domain;
using MediatR;

namespace GitIssuer.Application.Repository.Commands.CreateIssue;

public record CreateIssueCommand(
    ContentType ContentType,
    string Owner,
    string Repository,
    string Title,
    string? Body,
    string? Assignee,
    List<string>? Assignees,
    List<string>? Milestones,
    List<string>? Labels
    ): IRequest<long>;