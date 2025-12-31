using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Fakes;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Tools;

[TestFixture]
public class SummarizeTextToolTests
{
    [Test]
    public async Task ExecuteAsync_ReturnsSummary_FromCompletionService()
    {
        // Arrange
        var fakeCompletion = new FakeTextCompletionService();
        fakeCompletion.EnqueueResponse(
            "Service outage caused by retry storm; resolved by throttling.");

        var tool = new SummarizeTextTool(fakeCompletion);

        var input =
            "There was a service outage due to a retry storm that overloaded the system. " +
            "The issue was resolved by adding throttling.";

        // Act
        string result = await tool.ExecuteAsync(input);

        // Assert
        Assert.That(
            result,
            Is.EqualTo("Service outage caused by retry storm; resolved by throttling."),
            "Tool should return the summary produced by the completion service");
    }

    [Test]
    public void ExecuteAsync_ThrowsArgumentException_WhenInputIsEmpty()
    {
        // Arrange
        var fakeCompletion = new FakeTextCompletionService();
        var tool = new SummarizeTextTool(fakeCompletion);

        // Act + Assert
        Assert.ThrowsAsync<ArgumentException>(
            async () => await tool.ExecuteAsync(string.Empty),
            "Empty input should be rejected before calling the completion service");
    }
}
