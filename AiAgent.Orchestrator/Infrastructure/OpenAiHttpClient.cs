using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure
{
    public class OpenAiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAiOptions _options;

        public OpenAiHttpClient(HttpClient httpClient, OpenAiOptions options)
        {
            _httpClient = httpClient;
            _options = options;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        }

        public async Task<string> CreateChatCompletionAsync(
            string systemPrompt,
            string userPrompt)
        {
            var payload = new
            {
                model = _options.Model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                temperature = 0.0 // critical for determinism
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response =
                await _httpClient.PostAsync("/chat/completions", content);

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            return doc
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()
                ?? throw new InvalidOperationException("Empty OpenAI response");
        }
    }
}
