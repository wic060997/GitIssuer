using GitIssuer.Domain;
using GitIssuer.Provider.Github;
using GitIssuer.Provider.Gitlab;
using MediatR;

namespace GitIssuer.Application.Repository.Queries.GetRepositories;

public class GetRepositoriesQueryHandler(IGithubProvider githubProvider, IGitlabProvider gitlabProvider)
    : IRequestHandler<GetRepositoriesQuery, List<string>>
{
    public async Task<List<string>> Handle(GetRepositoriesQuery request, CancellationToken cancellationToken)
    {
        return request.ContentType switch
        {
            ContentType.Github => await githubProvider.GetRepositoriesAsync(request.Owner, cancellationToken),
            ContentType.Gitlab => await gitlabProvider.GetProjectsAsync(cancellationToken),
            _ => throw new ArgumentOutOfRangeException(
                nameof(request.ContentType),
                request.ContentType,
                "Unsupported content type")
        };
    }
}