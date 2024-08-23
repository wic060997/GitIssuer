using GitIssuer.Domain;

namespace GitIssuer.Api.Repository.Requests;

public record GetRepositoryIssuesRequest(
    ContentType ContentType,
    string Owner,
    int? Page,
    int? Limit
);