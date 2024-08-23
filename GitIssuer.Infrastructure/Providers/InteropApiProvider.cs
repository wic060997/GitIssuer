using System.Net;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace GitIssuer.Infrastructure.Providers;

public abstract class InteropApiProvider(
    HttpClient httpClient
)
{
    private readonly JsonSerializerSettings _jsonSerializerSettings =
        new() { NullValueHandling = NullValueHandling.Ignore };

    protected async Task<TResult> GetAsync<TResult>(
        string? requestUri,
        string headerName,
        string token,
        CancellationToken cancellationToken
    ) where TResult : new()
    {
        httpClient.DefaultRequestHeaders.Add($"{headerName}", $"{token}");

        using var response = await httpClient.GetAsync(requestUri, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResult>(content, _jsonSerializerSettings) ?? new TResult();
    }

    protected async Task<TResult> PostAsync<TResult>(
        string? requestUri,
        string headerName,
        string token,
        HttpContent? httpContent,
        CancellationToken cancellationToken) where TResult : new()
    {
        httpClient.DefaultRequestHeaders.Add($"{headerName}", $"{token}");

        using var response = await httpClient.PostAsync(requestUri, httpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResult>(content, _jsonSerializerSettings) ?? new TResult();
    }

    protected async Task<TResult> PutAsync<TResult>(
        string? requestUri,
        string headerName,
        string token,
        HttpContent? httpContent,
        CancellationToken cancellationToken
    ) where TResult : new()
    {
        httpClient.DefaultRequestHeaders.Add($"{headerName}", $"{token}");

        using var response = await httpClient.PutAsync(requestUri, httpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResult>(content, _jsonSerializerSettings) ?? new TResult();
    }

    protected async Task<TResult> PatchAsync<TResult>(
        string? requestUri,
        string headerName,
        string token,
        HttpContent? httpContent,
        CancellationToken cancellationToken) where TResult : new()
    {
        httpClient.DefaultRequestHeaders.Add($"{headerName}", $"{token}");

        using var response = await httpClient.PatchAsync(requestUri, httpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<TResult>(content, _jsonSerializerSettings) ?? new TResult();
    }
}