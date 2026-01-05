using System.Collections.Concurrent;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools
{
    public class ToolRegistry : IToolRegistry
    {
        private readonly ConcurrentDictionary<string, ITool> _tools = new();

        public ToolRegistry(IEnumerable<ITool> tools)
        {
            foreach (var tool in tools)
            {
                _tools[tool.Name] = tool;
            }
        }

        public void Register(ITool tool)
        {
            _tools[tool.Name] = tool;
        }

        public ITool? GetTool(string toolName)
        {
            _tools.TryGetValue(toolName, out var tool);
            return tool;
        }
    }
}