namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure
{
    public interface ILlmClient
    {
        Task<string> GetCompletionAsync(string systemPrompt, string userPrompt);
    }
}