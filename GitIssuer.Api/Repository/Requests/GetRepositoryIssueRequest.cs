using GitIssuer.Domain;

namespace GitIssuer.Api.Repository.Requests;

public record GetRepositoryIssueRequest(
    ContentType ContentType,
    string Owner
);