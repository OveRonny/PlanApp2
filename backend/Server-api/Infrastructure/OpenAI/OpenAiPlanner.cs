using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
namespace Server_api.Infrastructure.OpenAI;

public sealed class OpenAiPlanner(HttpClient httpClient, IOptions<OpenAiOptions> options) : IOpenAiPlanner
{
    private readonly OpenAiOptions settings = options.Value;
    public async Task<string> GenerateFeaturePlanAsync(string projectName, string projectDescription, IReadOnlyList<string> technologies, string request, CancellationToken cancellationToken)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/responses");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
        message.Content = JsonContent.Create(new { model = settings.Model, input = $"Du er en senior utvikler. Følg områdebegrensningen i brukerens behov helt strikt. Lag én feature med konkrete oppgaver for prosjektet {projectName}. Prosjektbeskrivelse: {projectDescription}. Teknologier: {string.Join(", ", technologies)}. Brukerens behov: {request}. Returner kun JSON med feltene title, description og tasks. tasks skal være en liste med title, description og context. Hver oppgave må tilhøre det valgte området; ikke bland inn oppgaver fra andre områder. Ikke finn på teknologier som ikke er oppgitt." });
        var response = await httpClient.SendAsync(message, cancellationToken); response.EnsureSuccessStatusCode();
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
        var outputText = json.RootElement.GetProperty("output").EnumerateArray().SelectMany(x => x.GetProperty("content").EnumerateArray()).FirstOrDefault(x => x.TryGetProperty("text", out _)).GetProperty("text").GetString();
        return outputText ?? throw new InvalidOperationException("OpenAI returnerte ikke en plan.");
    }
}
