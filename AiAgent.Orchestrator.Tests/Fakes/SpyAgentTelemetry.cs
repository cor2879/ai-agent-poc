using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Telemetry;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Fakes
{
    public class SpyAgentTelemetry : IAgentTelemetry
    {
        public List<string> Events { get; } = new();

        public void AgentInvoked(string userInput)
            => Events.Add("AgentInvoked");

        public void DecisionCreated(
            string? toolName,
            double confidence,
            bool requiresHumanReview,
            string reasoning)
            => Events.Add("DecisionCreated");

        public void ToolExecuted(string toolName, string? input)
            => Events.Add("ToolExecuted");

        public void ToolExecutionFailed(string toolName, string error)
            => Events.Add("ToolExecutionFailed");

        public void AgentFailed(string error)
            => Events.Add("AgentFailed");

        public void HumanReviewRequired(string reason)
            => Events.Add("HumanReviewRequired");
    }
}
