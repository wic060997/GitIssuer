using GitIssuer.Domain;
using GitIssuer.Provider.Github;
using GitIssuer.Provider.Gitlab;
using MediatR;

namespace GitIssuer.Application.Repository.Queries.GetRepositoryIssues;

public class GetRepositoryIssuesQueryHandler(IGithubProvider githubProvider, IGitlabProvider gitlabProvider)
    : IRequestHandler<GetRepositoryIssuesQuery, PaginationResponse<GetRepositoryIssuesResponse>>
{
    public async Task<PaginationResponse<GetRepositoryIssuesResponse>> Handle(GetRepositoryIssuesQuery request,
        CancellationToken cancellationToken)
    {
        var response = request.ContentType switch
        {
            ContentType.Github => await githubProvider.GetIssuesAsync(
                request.Owner,
                request.Repository,
                request.Take(),
                request.Skip(),
                cancellationToken),
            ContentType.Gitlab => await gitlabProvider.GetIssuersAsync(
                request.Repository,
                request.Take(),
                request.Skip(),
                cancellationToken
            ),
            _ => throw new ArgumentOutOfRangeException(
                nameof(request.ContentType),
                request.ContentType,
                "Unsupported content type"
            )
        };

        return new PaginationResponse<GetRepositoryIssuesResponse>(
            Data: response.Select(x => new GetRepositoryIssuesResponse(x.Id, x.Title, x.Body, x.State)).ToList(),
            Pagination: new PaginationModelResponse(
                Limit: request.Limit,
                Page: request.Page
            )
        );
    }
}