using GitIssuer.Domain;
using GitIssuer.Provider.Github;
using GitIssuer.Provider.Gitlab;
using MediatR;

namespace GitIssuer.Application.Repository.Queries.GetRepositoryIssue;

public class GetRepositoryIssueQueryHandler(IGithubProvider githubProvider, IGitlabProvider gitlabProvider)
    : IRequestHandler<GetRepositoryIssueQuery, GetRepositoryIssueResponse>
{
    public async Task<GetRepositoryIssueResponse> Handle(GetRepositoryIssueQuery request,
        CancellationToken cancellationToken)
    {
        var response = request.ContentType switch
        {
            ContentType.Github => await githubProvider.GetIssueAsync(
                request.Owner,
                request.Repository,
                request.IssueId,
                cancellationToken),

            ContentType.Gitlab => await gitlabProvider.GetIssueAsync(
                request.Repository,
                request.IssueId,
                cancellationToken),

            _ => throw new ArgumentOutOfRangeException(
                nameof(request.ContentType),
                request.ContentType,
                "Unsupported content type"
            )
        };

        return new GetRepositoryIssueResponse(response.Id, response.Title, response.Body, response.State);
    }
}