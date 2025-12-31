using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Fakes;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Agent
{
    [TestFixture]
    public class AgentEngineTests
    {
        [Test]
        public async Task MalformedLlmOutput_ForcesHumanReview_AndPreventsToolExecution()
        {
            // Arrange
            var fakeLlm = new FakeLlmClient();

            // Enqueue malformed (non-JSON) response
            fakeLlm.EnqueueResponse("this is not valid json");

            var decisionService = new LlmAgentDecisionService(fakeLlm);

            var spyTool = new SpyTool();
            var toolRegistry = new ToolRegistry(Array.Empty<ITool>());
            toolRegistry.Register(spyTool);
            var telemetry = new SpyAgentTelemetry();

            var agent = new AgentEngine(decisionService, toolRegistry, telemetry);

            // Act
            AgentResult result =
                await agent.HandleAsync("Summarize this incident report");

            // Assert
            Assert.That(result.RequiresHumanReview, Is.True,
                "Malformed LLM output should require human review");

            Assert.That(result.Confidence, Is.EqualTo(0.0),
                "Confidence should be zero on parsing failure");

            Assert.That(spyTool.WasExecuted, Is.False,
                "Tool execution must never occur on malformed LLM output");

            Assert.That(result.Reasoning, Does.Contain("Failed to parse"),
                "Reasoning should explain parsing failure");
        }

        [Test]
        public async Task ValidLlmDecision_ExecutesTool_AndReturnsResult()
        {
            // Arrange
            var fakeLlm = new FakeLlmClient();

            fakeLlm.EnqueueResponse("""
            {
            "toolName": "spy_tool",
            "toolInput": "summarize this incident",
            "confidence": 0.92,
            "requiresHumanReview": false,
            "reasoning": "User explicitly requested a summary"
            }
            """);

            var decisionService = new LlmAgentDecisionService(fakeLlm);

            var spyTool = new SpyTool();
            var toolRegistry = new ToolRegistry(Array.Empty<ITool>());
            toolRegistry.Register(spyTool);
            var telemetry = new SpyAgentTelemetry();

            var agent = new AgentEngine(decisionService, toolRegistry, telemetry);

            // Act
            AgentResult result =
                await agent.HandleAsync("Summarize this incident report");

            // Assert
            Assert.That(result.RequiresHumanReview, Is.False,
                "Valid decision should not require human review");

            Assert.That(result.Successful, Is.True,
                "Agent should succeed on valid decision");

            Assert.That(spyTool.WasExecuted, Is.True,
                "Tool should be executed on valid decision");

            Assert.That(result.Output, Is.EqualTo("Executed"),
                "Agent output should match tool output");

            Assert.That(result.Confidence, Is.EqualTo(0.92),
                "Confidence should flow through from decision");

            Assert.That(result.Reasoning, Does.Contain("requested a summary"),
                "Reasoning should be preserved from decision");
        }

        [Test]
        public async Task UnknownToolName_FailsGracefully_AndDoesNotExecuteAnyTool()
        {
            // Arrange
            var fakeLlm = new FakeLlmClient();

            fakeLlm.EnqueueResponse("""
            {
            "toolName": "non_existent_tool",
            "toolInput": "do something dangerous",
            "confidence": 0.77,
            "requiresHumanReview": false,
            "reasoning": "This tool would hypothetically satisfy the request"
            }
            """);

            var decisionService = new LlmAgentDecisionService(fakeLlm);

            var spyTool = new SpyTool();
            var toolRegistry = new ToolRegistry(Array.Empty<ITool>());
            toolRegistry.Register(spyTool); // note: spy_tool != non_existent_tool
            var telemetry = new SpyAgentTelemetry();

            var agent = new AgentEngine(decisionService, toolRegistry, telemetry);

            // Act
            AgentResult result =
                await agent.HandleAsync("Perform an unknown action");

            // Assert
            Assert.That(result.Successful, Is.False,
                "Agent should fail when tool is unknown");

            Assert.That(result.RequiresHumanReview, Is.False,
                "Unknown tool should fail gracefully, not require review");

            Assert.That(spyTool.WasExecuted, Is.False,
                "No tool should be executed when tool name is unknown");

            Assert.That(result.Output, Does.Contain("No registered tool found"),
                "Failure reason should explain missing tool");

            Assert.That(result.Confidence, Is.EqualTo(0.77),
                "Confidence should still flow through from decision");
        }
    }
}