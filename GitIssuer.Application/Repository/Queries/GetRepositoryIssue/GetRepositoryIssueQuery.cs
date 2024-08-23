using GitIssuer.Domain;
using MediatR;

namespace GitIssuer.Application.Repository.Queries.GetRepositoryIssue;

public record GetRepositoryIssueQuery(
    ContentType ContentType,
    string Owner,
    string Repository,
    long IssueId
) : IRequest<GetRepositoryIssueResponse>;