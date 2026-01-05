namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure
{
    public class OpenAiLlmClient : ILlmClient
    {
        private readonly OpenAiHttpClient _client;

        public OpenAiLlmClient(OpenAiHttpClient client)
        {
            _client = client;
        }

        public Task<string> GetCompletionAsync(
            string systemPrompt,
            string userPrompt)
        {
            return _client.CreateChatCompletionAsync(
                systemPrompt,
                userPrompt);
        }
    }
}
