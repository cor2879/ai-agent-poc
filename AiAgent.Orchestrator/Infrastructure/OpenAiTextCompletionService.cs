namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure
{
    public class OpenAiTextCompletionService : ITextCompletionService
    {
        private readonly OpenAiHttpClient _client;

        public OpenAiTextCompletionService(OpenAiHttpClient client)
        {
            _client = client;
        }

        public Task<string> GetCompletionAsync(string prompt)
        {
            return _client.CreateChatCompletionAsync(
                systemPrompt: "You are a helpful assistant.",
                userPrompt: prompt);
        }
    }
}
