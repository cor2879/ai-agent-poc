namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools
{
    public interface IToolRegistry
    {
        ITool? GetTool(string toolName);

        void Register(ITool tool);
    }
}
