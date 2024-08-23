namespace GitIssuer.Provider.Gitlab.UpdateIssue;

public record UpdateGitlabIssueModel(
    string ProjectId,
    long IssueId,
    string? Title = null,
    string? Description = null,
    string? State = null
);