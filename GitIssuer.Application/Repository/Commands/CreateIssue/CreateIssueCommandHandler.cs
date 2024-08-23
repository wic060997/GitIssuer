using GitIssuer.Domain;
using GitIssuer.Provider.Github;
using GitIssuer.Provider.Github.CreateIssue;
using GitIssuer.Provider.Gitlab;
using GitIssuer.Provider.Gitlab.CreateIssue;
using MediatR;

namespace GitIssuer.Application.Repository.Commands.CreateIssue;

public class CreateIssueCommandHandler(
    IGithubProvider githubProvider,
    IGitlabProvider gitlabProvider
)
    : IRequestHandler<CreateIssueCommand, long>
{
    public async Task<long> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        return request.ContentType switch
        {
            ContentType.Github => await githubProvider.CreateIssueAsync(
                new CreateGithubIssueModel(
                    request.Owner,
                    request.Repository,
                    request.Title,
                    request.Body
                ), cancellationToken),

            ContentType.Gitlab => await gitlabProvider.CreateIssueAsync(
                new CreateGitlabIssueModel(request.Repository, request.Title, request.Body),
                cancellationToken
            ),

            _ => throw new ArgumentOutOfRangeException(nameof(request.ContentType), request.ContentType,
                "Unsupported content type")
        };
    }
}