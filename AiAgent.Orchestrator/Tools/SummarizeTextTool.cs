using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools
{
    public class SummarizeTextTool : ITool
    {
        private readonly ITextCompletionService _completionService;

        public SummarizeTextTool(ITextCompletionService completionService)
        {
            _completionService = completionService;
        }

        public string Name => "summarize_text";

        public string Description =>
            "Summarizes a block of text into a concise, high-level overview.";

        public async Task<string> ExecuteAsync(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input text cannot be empty.");

            var prompt = $"""
            Summarize the following text clearly and concisely.
            Focus on key points and actionable information.

            Text:
            {input}
            """;

            return await _completionService.GetCompletionAsync(prompt);
        }
    }
}
