namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure
{
    public class OpenAiOptions
    {
        public string ApiKey { get; init; } = string.Empty;
        public string Model { get; init; } = "gpt-4o-mini";
        public string BaseUrl { get; init; } = "https://api.openai.com/v1";
    }
}
