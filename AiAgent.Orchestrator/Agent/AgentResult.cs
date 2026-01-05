namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent
{
    public class AgentResult
    {
        public bool Successful { get; private init; }
        public bool RequiresHumanReview { get; private init; }
        public string Output { get; private init; } = string.Empty;
        public double Confidence { get; private init; }
        public string Reasoning { get; private init; } = string.Empty;

        public static AgentResult Success(string output, double confidence, string reasoning)
            => new()
            {
                Successful = true,
                Output = output,
                Confidence = confidence,
                Reasoning = reasoning
            };

        public static AgentResult NeedsReview(string reasoning, double confidence)
            => new()
            {
                RequiresHumanReview = true,
                Reasoning = reasoning,
                Confidence = confidence
            };

        public static AgentResult Failed(string message, double confidence)
            => new()
            {
                Successful = false,
                Output = message,
                Confidence = confidence
            };
    }
}
