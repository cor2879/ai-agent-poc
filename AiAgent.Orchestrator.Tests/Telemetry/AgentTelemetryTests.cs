using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Fakes;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Telemetry;

[TestFixture]
public class AgentTelemetryTests
{
    [Test]
    public async Task HappyPath_EmitsExpectedTelemetryEvents_InOrder()
    {
        // Arrange
        var fakeLlm = new FakeLlmClient();

        fakeLlm.EnqueueResponse("""
        {
          "toolName": "spy_tool",
          "toolInput": "test input",
          "confidence": 0.9,
          "requiresHumanReview": false,
          "reasoning": "Clear intent"
        }
        """);

        var decisionService = new LlmAgentDecisionService(fakeLlm);

        var spyTool = new SpyTool();
        var registry = new ToolRegistry(Array.Empty<ITool>());
        registry.Register(spyTool);

        var telemetry = new SpyAgentTelemetry();

        var agent = new AgentEngine(
            decisionService,
            registry,
            telemetry);

        // Act
        await agent.HandleAsync("Run the spy tool");

        // Assert
        Assert.That(
            telemetry.Events,
            Is.EqualTo(new[]
            {
                "AgentInvoked",
                "DecisionCreated",
                "ToolExecuted"
            }),
            "Telemetry events should be emitted in correct order for happy path");
    }
}
