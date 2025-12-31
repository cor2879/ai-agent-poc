using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Fakes
{
    public class SpyTool : ITool
    {
        public bool WasExecuted { get; private set; }

        public string Name => "spy_tool";

        public string Description => "Test spy tool";

        public async Task<string> ExecuteAsync(string input)
        {
            WasExecuted = true;
            return await Task.FromResult("Executed");
        }
    }
}
