namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools
{
    public interface ITool
    {
        /// <summary>
        /// Stable name used by the agent to reference this tool.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Human-readable description used by the agent for decision-making.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Executes the tool deterministically using validated input.
        /// </summary>
        Task<string> ExecuteAsync(string input);
    }
}
