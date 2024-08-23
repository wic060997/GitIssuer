using System.Text;
using GitIssuer.Domain.Issue;
using GitIssuer.Provider.Github;
using GitIssuer.Provider.Github.CreateIssue;
using GitIssuer.Provider.Github.UpdateIssue;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GitIssuer.Infrastructure.Providers;

public class GithubProvider(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory
) : InteropApiProvider(httpClientFactory.CreateClient("GithubProvider")), IGithubProvider
{
    private string GetToken => configuration["Credentials:Github:Token"] ??
                               throw new NullReferenceException("Github token not found");

    private const string HeaderName = "Authorization";

    public async Task<List<string>> GetRepositoriesAsync(string userName, CancellationToken cancellationToken)
    {
        var result = await GetAsync<List<GithubRepositoryModel>>(
            $"/users/{userName}/repos",
            HeaderName,
            $"Bearer {GetToken}",
            cancellationToken);
        return result.Select(x => x.Name).ToList();
    }

    public async Task<List<IssueModel>> GetIssuesAsync(
        string owner,
        string repository,
        int limit,
        int page,
        CancellationToken cancellationToken)
    {
        var result = await GetAsync<List<GithubIssueModel>>(
            $"/repos/{owner}/{repository}/issues?per_page={limit}&page={page}",
            HeaderName,
            $"Bearer {GetToken}",
            cancellationToken);

        return result.Select(x => new IssueModel( x.Number, x.Title, x.Body, x.State)).ToList();
    }

    public async Task<IssueModel> GetIssueAsync(
        string owner,
        string repository,
        long issueId,
        CancellationToken cancellationToken)
    {
        var result = await GetAsync<GithubIssueModel>(
            $"/repos/{owner}/{repository}/issues/{issueId}",
            HeaderName,
            $"Bearer {GetToken}",
            cancellationToken);

        return new IssueModel( result.Number, result.Title, result.Body, result.State);
    }

    public async Task<long> CreateIssueAsync(CreateGithubIssueModel model, CancellationToken cancellationToken)
    {
        var issueContent = new StringContent(
            JsonConvert.SerializeObject(new
                {
                    title = model.Title,
                    body = model.Body
                },
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }),
            Encoding.UTF8,
            "application/json");

        var response = await PostAsync<GithubIssueModel>(
            $"repos/{model.Owner}/{model.Repository}/issues",
            HeaderName,
            $"Bearer {GetToken}",
            issueContent,
            cancellationToken
        );

        return response.Number;
    }

    public async Task UpdateIssueAsync(UpdateGithubIssueModel model, CancellationToken cancellationToken)
    {
        var issueContent = new StringContent(
            JsonConvert.SerializeObject(new
                {
                    title = model.Title,
                    body = model.Body,
                    state = model.State
                },
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }),
            Encoding.UTF8,
            "application/json");

        await PatchAsync<GithubIssueModel>(
            $"repos/{model.Owner}/{model.Repository}/issues/{model.IssueNumber}",
            HeaderName,
            $"Bearer {GetToken}",
            issueContent,
            cancellationToken
        );
    }
}