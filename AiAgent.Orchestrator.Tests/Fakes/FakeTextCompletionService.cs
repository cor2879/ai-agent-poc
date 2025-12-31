using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Fakes
{
    public class FakeTextCompletionService : ITextCompletionService
    {
        private readonly Queue<string> _responses = new();

        public void EnqueueResponse(string response)
        {
            _responses.Enqueue(response);
        }

        public async Task<string> GetCompletionAsync(string prompt)
        {
            if (!_responses.Any())
                throw new InvalidOperationException(
                    "No fake completion responses configured.");

            return await Task.FromResult(_responses.Dequeue());
        }
    }
}
