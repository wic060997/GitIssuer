namespace GitIssuer.Provider.Gitlab.CreateIssue;

public record CreateGitlabIssueModel(
    string ProjectId,
    string Title,
    string? Description
);