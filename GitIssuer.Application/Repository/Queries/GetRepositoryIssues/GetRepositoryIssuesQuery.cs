using GitIssuer.Domain;
using MediatR;

namespace GitIssuer.Application.Repository.Queries.GetRepositoryIssues;

public record GetRepositoryIssuesQuery(
    ContentType ContentType,
    string Owner,
    string Repository,
    int? Page,
    int? Limit
) : PaginationQuery(Page, Limit), IRequest<PaginationResponse<GetRepositoryIssuesResponse>>;