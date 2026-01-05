namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure
{
    public interface ITextCompletionService
    {
        Task<string> GetCompletionAsync(string prompt);
    }
}
