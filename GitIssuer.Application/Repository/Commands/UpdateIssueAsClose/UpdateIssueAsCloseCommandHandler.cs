using GitIssuer.Domain;
using GitIssuer.Provider.Github;
using GitIssuer.Provider.Github.UpdateIssue;
using GitIssuer.Provider.Gitlab;
using GitIssuer.Provider.Gitlab.UpdateIssue;
using MediatR;

namespace GitIssuer.Application.Repository.Commands.UpdateIssueAsClose;

public class UpdateIssueAsCloseCommandHandler(
    IGithubProvider githubProvider,
    IGitlabProvider gitlabProvider
) : IRequestHandler<UpdateIssueAsCloseCommand, Unit>
{
    public async Task<Unit> Handle(UpdateIssueAsCloseCommand request, CancellationToken cancellationToken)
    {
        switch (request.ContentType)
        {
            case ContentType.Github:
                await githubProvider.UpdateIssueAsync(
                    new UpdateGithubIssueModel(
                        Owner: request.Owner,
                        Repository: request.Repository,
                        IssueNumber: request.IssueId,
                        State: GithubState.Closed
                    ),
                    cancellationToken
                );
                break;

            case ContentType.Gitlab:
                await gitlabProvider.UpdateIssueAsync(
                    new UpdateGitlabIssueModel(
                        ProjectId: request.Repository,
                        IssueId: request.IssueId,
                        State: "close"
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