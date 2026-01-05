namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure
{
    public class OpenAiLlmClient : ILlmClient
    {
        private readonly OpenAiHttpClient _client;

        public OpenAiLlmClient(OpenAiHttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetCompletionAsync(
            string systemPrompt,
            string userPrompt)
        {
            return await _client.CreateChatCompletionAsync(
                systemPrompt,
                userPrompt);
        }
    }
}
