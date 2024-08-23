namespace GitIssuer.Domain.Issue;

public record IssueModel(
    long Id,
    string Title,
    string Body,
    string State
);