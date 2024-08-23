using GitIssuer.Domain;
using GitIssuer.Provider.Github;
using GitIssuer.Provider.Github.UpdateIssue;
using GitIssuer.Provider.Gitlab;
using GitIssuer.Provider.Gitlab.UpdateIssue;
using MediatR;

namespace GitIssuer.Application.Repository.Commands.UpdateIssue;

public class UpdateIssueCommandHandler(
    IGithubProvider githubProvider,
    IGitlabProvider gitlabProvider
) : IRequestHandler<UpdateIssueCommand, Unit>
{
    public async Task<Unit> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
    {
        switch (request.ContentType)
        {
            case ContentType.Github:
                await githubProvider.UpdateIssueAsync(
                    new UpdateGithubIssueModel(
                        request.Owner,
                        request.Repository,
                        request.IssueId,
                        request.Title,
                        request.State,
                        request.Body
                    ),
                    cancellationToken
                );
                break;

            case ContentType.Gitlab:
                await gitlabProvider.UpdateIssueAsync(
                    new UpdateGitlabIssueModel(
                        ProjectId: request.Repository,
                        IssueId: request.IssueId,
                        Title: request.Title,
                        Description: request.Body
                    ),
                    cancellationToken);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(request.ContentType), request.ContentType,
                    "Unsupported content type");
        }

        return Unit.Value;
    }
}