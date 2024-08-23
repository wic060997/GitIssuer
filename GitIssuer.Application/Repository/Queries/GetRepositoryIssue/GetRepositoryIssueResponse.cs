namespace GitIssuer.Application.Repository.Queries.GetRepositoryIssue;

public record GetRepositoryIssueResponse(
    long Id,
    string Title,
    string Body,
    string State
);