using System.Text.Json;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Telemetry
{
    public class ConsoleAgentTelemetry : IAgentTelemetry
    {
        private static void Write(string eventName, object payload)
        {
            var envelope = new
            {
                Timestamp = DateTime.UtcNow,
                Event = eventName,
                Payload = payload
            };

            Console.WriteLine(JsonSerializer.Serialize(envelope));
        }

        public void AgentInvoked(string userInput)
            => Write("AgentInvoked", new { userInput });

        public void DecisionCreated(
            string? toolName,
            double confidence,
            bool requiresHumanReview,
            string reasoning)
            => Write("DecisionCreated", new
            {
                toolName,
                confidence,
                requiresHumanReview,
                reasoning
            });

        public void ToolExecuted(string toolName, string? input)
            => Write("ToolExecuted", new { toolName, input });

        public void ToolExecutionFailed(string toolName, string error)
            => Write("ToolExecutionFailed", new { toolName, error });

        public void AgentFailed(string error)
            => Write("AgentFailed", new { error });

        public void HumanReviewRequired(string reason)
            => Write("HumanReviewRequired", new { reason });
    }
}
