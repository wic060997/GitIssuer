using GitIssuer.Domain;

namespace GitIssuer.Api.Repository.Requests;

public record UpdateRepositoryIssueAsClosedRequest(
    ContentType ContentType,
    string Owner
);