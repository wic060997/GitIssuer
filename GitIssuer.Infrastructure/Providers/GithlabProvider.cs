using System.Text;
using GitIssuer.Domain.Issue;
using GitIssuer.Provider.Gitlab;
using GitIssuer.Provider.Gitlab.CreateIssue;
using GitIssuer.Provider.Gitlab.UpdateIssue;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GitIssuer.Infrastructure.Providers;

public class GitlabProvider(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory
) : InteropApiProvider(httpClientFactory.CreateClient("GitlabProvider")), IGitlabProvider
{
    private string GetToken => configuration["Credentials:Gitlab:Token"] ??
                               throw new NullReferenceException("Gitlab token not found");

    private const string HeaderName = "Authorization";
    
    private string BaseUrl => configuration["Credentials:Gitlab:ApiUrl"] ??throw new NullReferenceException("Gitlab ApiUrl not found");

    public async Task<List<string>> GetProjectsAsync(CancellationToken cancellationToken)
    {
        var result = await GetAsync<List<GitlabProjectModel>>(
            $"{BaseUrl}/projects",
            HeaderName,
            $"Bearer {GetToken}",
            cancellationToken);
        return result.Select(x => x.Id.ToString()).ToList();
    }

    public async Task<List<IssueModel>> GetIssuersAsync(
        string projectId,
        int limit,
        int page, 
        CancellationToken cancellationToken)
    {
        var result = await GetAsync<List<GitlabIssueModel>>(
            $"{BaseUrl}/projects/{projectId}/issues?per_page={limit}&page={page}",
            HeaderName,
            $"Bearer {GetToken}",
            cancellationToken);

        return result.Select(x => new IssueModel(x.Id, x.Title, x.Description, x.State)).ToList();
    }

    public async Task<IssueModel> GetIssueAsync(string projectId, long issueId, CancellationToken cancellationToken)
    {
        var result = await GetAsync<GitlabIssueModel>(
            $"{BaseUrl}/projects/{projectId}/issues/{issueId}",
            HeaderName,
            $"Bearer {GetToken}",
            cancellationToken);

        return new IssueModel(result.Id, result.Title, result.Description, result.State);
    }

    public async Task<long> CreateIssueAsync(CreateGitlabIssueModel model, CancellationToken cancellationToken)
    {
        var issueContent = new StringContent(
            JsonConvert.SerializeObject(new
                {
                    title = model.Title,
                    description = model.Description
                },
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }),
            Encoding.UTF8,
            "application/json");

        var response = await PostAsync<GitlabIssueModel>(
            $"{BaseUrl}/projects/{model.ProjectId}/issues",
            HeaderName,
            $"Bearer {GetToken}",
            issueContent,
            cancellationToken
        );

        return response.Id;
    }

    public async Task UpdateIssueAsync(UpdateGitlabIssueModel model, CancellationToken cancellationToken)
    {
        var issueContent = new StringContent(
            JsonConvert.SerializeObject(new
                {
                    title = model.Title,
                    description = model.Description,
                    state_event = model.State
                },
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }),
            Encoding.UTF8,
            "application/json");

        await PutAsync<GitlabIssueModel>(
            $"{BaseUrl}/projects/{model.ProjectId}/issues/{model.IssueId}",
            HeaderName,
            $"Bearer {GetToken}",
            issueContent,
            cancellationToken
        );
    }
}