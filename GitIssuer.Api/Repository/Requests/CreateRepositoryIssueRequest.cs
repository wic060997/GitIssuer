using GitIssuer.Domain;

namespace GitIssuer.Api.Repository.Requests;

public record CreateRepositoryIssueRequest(
    ContentType ContentType,
    string Owner,
    string Title,
    string? Body,
    string? Assignee,
    List<string>? Assignees,
    List<string>? Milestones,
    List<string>? Labels
);