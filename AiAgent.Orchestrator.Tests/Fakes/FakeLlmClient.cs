using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Fakes
{
    public class FakeLlmClient : ILlmClient
    {
        private readonly Queue<string> _responses = new();

        public void EnqueueResponse(string response)
        {
            _responses.Enqueue(response);
        }

        public async Task<string> GetCompletionAsync(string systemPrompt, string userPrompt)
        {
            if (!_responses.Any())
            {
                throw new InvalidOperationException(
                    "No fake LLM responses configured.");
            }

            return await Task.FromResult(_responses.Dequeue());
        }
    }
}
