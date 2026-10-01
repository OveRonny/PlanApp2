using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
namespace Server_api.Infrastructure.GitHub;
public sealed class GitHubClient(HttpClient httpClient, IOptions<GitHubOptions> options) : IGitHubClient
{
    private readonly GitHubOptions settings = options.Value;
    public async Task<GitHubProfile> ExchangeCodeAsync(string code, CancellationToken cancellationToken)
    { using var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token") { Content = JsonContent.Create(new { client_id = settings.ClientId, client_secret = settings.ClientSecret, code, redirect_uri = settings.RedirectUri }) }; tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); var tokenResponse = await httpClient.SendAsync(tokenRequest, cancellationToken); tokenResponse.EnsureSuccessStatusCode(); var token = await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken) ?? throw new InvalidOperationException("GitHub did not return an access token."); var profile = await Send<ProfileResponse>("https://api.github.com/user", token.AccessToken, cancellationToken); return new GitHubProfile(profile.Id.ToString(), profile.Login, profile.Name, profile.AvatarUrl, token.AccessToken); }
    public async Task<IReadOnlyList<GitHubRepositoryDto>> GetRepositoriesAsync(string accessToken, CancellationToken cancellationToken) { var items = await Send<List<RepositoryResponse>>("https://api.github.com/user/repos?per_page=100", accessToken, cancellationToken); return items.Select(x => new GitHubRepositoryDto(x.Id, x.FullName, x.DefaultBranch)).ToList(); }
    public async Task<IReadOnlyList<GitHubCollaboratorDto>> GetCollaboratorsAsync(string accessToken, string fullName, CancellationToken cancellationToken) { var items = await Send<List<CollaboratorResponse>>($"https://api.github.com/repos/{fullName}/collaborators?per_page=100", accessToken, cancellationToken); return items.Select(x => new GitHubCollaboratorDto(x.Id, x.Login, x.Name, x.AvatarUrl)).ToList(); }
    private async Task<T> Send<T>(string url, string token, CancellationToken cancellationToken) { using var request = new HttpRequestMessage(HttpMethod.Get, url); request.Headers.UserAgent.Add(new ProductInfoHeaderValue("PlanApp", "1.0")); request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token); var response = await httpClient.SendAsync(request, cancellationToken); response.EnsureSuccessStatusCode(); return await response.Content.ReadFromJsonAsync<T>(cancellationToken) ?? throw new InvalidOperationException("GitHub returned an empty response."); }
    private sealed record TokenResponse([property: JsonPropertyName("access_token")] string AccessToken);
    private sealed record ProfileResponse(long Id, string Login, [property: JsonPropertyName("name")] string? Name, [property: JsonPropertyName("avatar_url")] string? AvatarUrl);
    private sealed record RepositoryResponse(long Id, [property: JsonPropertyName("full_name")] string FullName, [property: JsonPropertyName("default_branch")] string? DefaultBranch);
    private sealed record CollaboratorResponse(long Id, string Login, [property: JsonPropertyName("name")] string? Name, [property: JsonPropertyName("avatar_url")] string? AvatarUrl);
}
