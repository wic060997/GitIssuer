using GitIssuer.Domain.Issue;
using GitIssuer.Provider.Github.CreateIssue;
using GitIssuer.Provider.Github.UpdateIssue;

namespace GitIssuer.Provider.Github;

public interface IGithubProvider
{
    Task<List<string>> GetRepositoriesAsync(string owner, CancellationToken cancellationToken);

    Task<List<IssueModel>> GetIssuesAsync(
        string owner,
        string repository,
        int limit,
        int page,
        CancellationToken cancellationToken);

    Task<IssueModel> GetIssueAsync(
        string owner,
        string repository,
        long issueId,
        CancellationToken cancellationToken
    );

    Task<long> CreateIssueAsync(CreateGithubIssueModel model, CancellationToken cancellationToken);

    Task UpdateIssueAsync(UpdateGithubIssueModel model, CancellationToken cancellationToken);
}