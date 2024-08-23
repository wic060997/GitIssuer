namespace GitIssuer.Provider.Github.UpdateIssue;

public record UpdateGithubIssueModel(
    string Owner,
    string Repository,
    long IssueNumber,
    string? Title = null,
    string? State = null,
    string? Body = null
);