using GitIssuer.Domain.Issue;
using GitIssuer.Provider.Gitlab.CreateIssue;
using GitIssuer.Provider.Gitlab.UpdateIssue;

namespace GitIssuer.Provider.Gitlab;

public interface IGitlabProvider
{
    Task<List<string>> GetProjectsAsync(CancellationToken cancellationToken);

    Task<List<IssueModel>> GetIssuersAsync(
        string projectId,
        int limit,
        int page,
        CancellationToken cancellationToken
    );

    Task<IssueModel> GetIssueAsync(
        string projectId,
        long issueId,
        CancellationToken cancellationToken
    );

    Task<long> CreateIssueAsync(CreateGitlabIssueModel model, CancellationToken cancellationToken);

    Task UpdateIssueAsync(UpdateGitlabIssueModel model, CancellationToken cancellationToken);
}