using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Fakes;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Agent;

[TestFixture]
public class CompositeFlowTests
{
    [Test]
    public async Task ClassifyThenSummarize_CompletesCompositeAgentFlow()
    {
        // Arrange
        var fakeLlm = new FakeLlmClient();

        // Step 1: Classification decision
        fakeLlm.EnqueueResponse("""
        {
          "toolName": "classify_intent",
          "toolInput": "Please summarize this incident report",
          "confidence": 0.85,
          "requiresHumanReview": false,
          "reasoning": "User intent needs classification first"
        }
        """);

        // Step 2: Summarization decision
        fakeLlm.EnqueueResponse("""
        {
          "toolName": "summarize_text",
          "toolInput": "Service outage caused by retry storm. Mitigated via throttling.",
          "confidence": 0.93,
          "requiresHumanReview": false,
          "reasoning": "Summarization requested"
        }
        """);

        var decisionService = new LlmAgentDecisionService(fakeLlm);

        var fakeCompletion = new FakeTextCompletionService();
        fakeCompletion.EnqueueResponse("Summarization");
        fakeCompletion.EnqueueResponse(
            "Service outage caused by retry storm; resolved with throttling.");

        var registry = new ToolRegistry(Array.Empty<ITool>());
        registry.Register(new ClassifyIntentTool(fakeCompletion));
        registry.Register(new SummarizeTextTool(fakeCompletion));

        var telemetry = new SpyAgentTelemetry();

        var agent = new AgentEngine(decisionService, registry, telemetry);

        // Act — Step 1: classify intent
        AgentResult classifyResult =
            await agent.HandleAsync("Please summarize this incident report");

        // Assert step 1
        Assert.That(classifyResult.Successful, Is.True);
        Assert.That(classifyResult.Output, Is.EqualTo("Summarization"));

        // Act — Step 2: summarize
        AgentResult summarizeResult =
            await agent.HandleAsync(
                "Service outage caused by retry storm. Mitigated via throttling.");

        // Assert step 2
        Assert.That(summarizeResult.Successful, Is.True);
        Assert.That(
            summarizeResult.Output,
            Is.EqualTo("Service outage caused by retry storm; resolved with throttling."));

        Assert.That(summarizeResult.Confidence, Is.EqualTo(0.93));
    }
}
