using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
namespace Server_api.Infrastructure.GitHub;

public sealed class GitHubClient(HttpClient httpClient, IOptions<GitHubOptions> options) : IGitHubClient
{
    private readonly GitHubOptions settings = options.Value;
    public async Task<GitHubProfile> ExchangeCodeAsync(string code, CancellationToken cancellationToken)
    { var tokenResponse = await httpClient.PostAsJsonAsync("https://github.com/login/oauth/access_token", new { client_id = settings.ClientId, client_secret = settings.ClientSecret, code, redirect_uri = settings.RedirectUri }, cancellationToken); tokenResponse.EnsureSuccessStatusCode(); var token = await tokenResponse.Content.ReadFromJsonAsync<GitHubTokenResponse>(cancellationToken) ?? throw new InvalidOperationException("GitHub did not return an access token."); using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user"); request.Headers.UserAgent.Add(new ProductInfoHeaderValue("PlanApp", "1.0")); request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken); var response = await httpClient.SendAsync(request, cancellationToken); response.EnsureSuccessStatusCode(); var profile = await response.Content.ReadFromJsonAsync<GitHubProfileResponse>(cancellationToken) ?? throw new InvalidOperationException("GitHub did not return a user profile."); return new GitHubProfile(profile.Id.ToString(), profile.Login, profile.Name, profile.AvatarUrl, token.AccessToken); }
    private sealed record GitHubTokenResponse(string AccessToken, string? Scope, string? TokenType);
    private sealed record GitHubProfileResponse(long Id, string Login, string? Name, string? AvatarUrl);
}
