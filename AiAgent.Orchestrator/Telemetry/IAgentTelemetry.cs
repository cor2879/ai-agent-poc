namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Telemetry
{
    public interface IAgentTelemetry
    {
        void AgentInvoked(string userInput);

        void DecisionCreated(
            string? toolName,
            double confidence,
            bool requiresHumanReview,
            string reasoning);

        void ToolExecuted(string toolName, string? input);

        void ToolExecutionFailed(string toolName, string error);

        void AgentFailed(string error);

        void HumanReviewRequired(string reason);
    }
}
