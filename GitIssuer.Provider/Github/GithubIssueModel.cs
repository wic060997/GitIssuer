using Newtonsoft.Json;

namespace GitIssuer.Provider.Github;

public record GithubIssueModel(
    [property: JsonProperty(PropertyName = "number")]
    long Number,
    [property: JsonProperty(PropertyName = "state")]
    string State,
    [property: JsonProperty(PropertyName = "title")]
    string Title,
    [property: JsonProperty(PropertyName = "body")]
    string Body
)
{
    public GithubIssueModel() : this(
        Number: default,
        State: string.Empty,
        Title: string.Empty,
        Body: string.Empty
    )
    {
    }
}