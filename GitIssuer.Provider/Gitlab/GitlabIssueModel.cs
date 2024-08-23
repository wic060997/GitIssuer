using Newtonsoft.Json;

namespace GitIssuer.Provider.Gitlab;

public record GitlabIssueModel(
    [property: JsonProperty(PropertyName = "iid")]
    int Id,
    [property: JsonProperty(PropertyName = "state")]
    string State,
    [property: JsonProperty(PropertyName = "title")]
    string Title,
    [property: JsonProperty(PropertyName = "description")]
    string Description
)
{
    public GitlabIssueModel() : this(
        Id: default,
        State: string.Empty,
        Title: string.Empty,
        Description: string.Empty
    )
    {
    }
}