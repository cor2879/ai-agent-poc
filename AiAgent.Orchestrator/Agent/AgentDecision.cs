using System.Text.Json.Serialization;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent
{
    public class AgentDecision
{
    public string? ToolName { get; init; }
    public string? ToolInput { get; init; }
    public double Confidence { get; init; }
    public bool RequiresHumanReview { get; init; }
    public string Reasoning { get; init; } = string.Empty;

    // Diagnostics (non-LLM-facing)
    [JsonIgnore]
    public Exception? Exception { get; init; } = null;
}
}