namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent
{
    public interface IAgentDecisionService
    {
        Task<AgentDecision> CreateDecisionAsync(string userInput);
    }
}
