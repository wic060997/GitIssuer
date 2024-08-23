namespace GitIssuer.Provider.Github.CreateIssue;

public record CreateGithubIssueModel(
    string Owner,
    string Repository,
    string Title,
    string? Body
);