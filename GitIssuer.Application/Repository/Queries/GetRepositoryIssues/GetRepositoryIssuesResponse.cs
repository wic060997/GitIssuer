namespace GitIssuer.Application.Repository.Queries.GetRepositoryIssues;

public record GetRepositoryIssuesResponse(
    long Id,
    string Title,
    string Body,
    string State
);