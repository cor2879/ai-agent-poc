using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Telemetry;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent
{
    public class AgentEngine
    {
        private readonly IAgentDecisionService _decisionService;
        private readonly IToolRegistry _toolRegistry;
        private readonly IAgentTelemetry _telemetry;


        public AgentEngine(
            IAgentDecisionService decisionService,
            IToolRegistry toolRegistry,
            IAgentTelemetry telemetry)
        {
            _decisionService = decisionService;
            _toolRegistry = toolRegistry;
            _telemetry = telemetry;
        }

        public async Task<AgentResult> HandleAsync(string userInput)
        {
            _telemetry.AgentInvoked(userInput);

            AgentDecision decision;

            try
            {
                decision = await _decisionService.CreateDecisionAsync(userInput);
            }
            catch (Exception ex)
            {
                _telemetry.AgentFailed(ex.Message);
                throw;
            }

            _telemetry.DecisionCreated(
                decision.ToolName,
                decision.Confidence,
                decision.RequiresHumanReview,
                decision.Reasoning);

            if (decision.RequiresHumanReview)
            {
                _telemetry.HumanReviewRequired(decision.Reasoning);

                return AgentResult.NeedsReview(
                    decision.Reasoning,
                    decision.Confidence);
            }

            if (string.IsNullOrWhiteSpace(decision.ToolName))
            {
                _telemetry.AgentFailed("No tool selected");
                return AgentResult.Failed("No tool selected", decision.Confidence);
            }

            var tool = _toolRegistry.GetTool(decision.ToolName);

            if (tool == null)
            {
                _telemetry.ToolExecutionFailed(
                    decision.ToolName,
                    "Tool not registered");

                return AgentResult.Failed(
                    $"No registered tool found for '{decision.ToolName}'.",
                    decision.Confidence);
            }

            try
            {
                _telemetry.ToolExecuted(decision.ToolName, decision.ToolInput);

                var result = await tool.ExecuteAsync(decision.ToolInput);

                return AgentResult.Success(
                    result,
                    decision.Confidence,
                    decision.Reasoning);
            }
            catch (Exception ex)
            {
                _telemetry.ToolExecutionFailed(
                    decision.ToolName,
                    ex.Message);

                return AgentResult.Failed(
                    ex.Message,
                    decision.Confidence);
            }
        }
    }
}
