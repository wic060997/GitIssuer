using GitIssuer.Domain;

namespace GitIssuer.Api.Repository.Requests;

public record UpdateRepositoryIssueRequest(
    ContentType ContentType,
    string Owner,
    string? State,
    string? StateReason,
    string? Title,
    string? Body,
    List<string>? Assignees,
    List<string>? Milestones,
    List<string>? Labels
);