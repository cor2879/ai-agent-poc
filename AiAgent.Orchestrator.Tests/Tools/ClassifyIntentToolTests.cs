using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Fakes;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tests.Tools;

[TestFixture]
public class ClassifyIntentToolTests
{
    [Test]
    public async Task ExecuteAsync_ReturnsParsedIntent_WhenResponseIsValid()
    {
        // Arrange
        var fakeCompletion = new FakeTextCompletionService();
        fakeCompletion.EnqueueResponse("Summarization");

        var tool = new ClassifyIntentTool(fakeCompletion);

        // Act
        string result = await tool.ExecuteAsync(
            "Please summarize this incident report");

        // Assert
        Assert.That(result, Is.EqualTo(IntentType.Summarization.ToString()));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsUnknown_WhenResponseIsInvalid()
    {
        // Arrange
        var fakeCompletion = new FakeTextCompletionService();
        fakeCompletion.EnqueueResponse("TotallyNotAnIntent");

        var tool = new ClassifyIntentTool(fakeCompletion);

        // Act
        string result = await tool.ExecuteAsync(
            "Do something ambiguous");

        // Assert
        Assert.That(result, Is.EqualTo(IntentType.Unknown.ToString()));
    }
}
