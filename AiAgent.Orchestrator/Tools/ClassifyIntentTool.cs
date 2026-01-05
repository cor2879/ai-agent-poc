using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools
{
    public class ClassifyIntentTool : ITool
    {
        private readonly ITextCompletionService _completionService;

        public ClassifyIntentTool(ITextCompletionService completionService)
        {
            _completionService = completionService;
        }

        public string Name => "classify_intent";

        public string Description =>
            "Classifies the user's intent into a small set of known categories.";

        public async Task<string> ExecuteAsync(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input text cannot be empty.");
            }

            var prompt = $"""
            Classify the intent of the following user request into ONE of these values:

            - Summarization
            - Analysis
            - Question
            - ActionRequest
            - Unknown

            Respond with ONLY the intent value.

            User request:
            {input}
            """;

            var response = await _completionService.GetCompletionAsync(prompt);

            // Normalize + guardrail
            if (Enum.TryParse<IntentType>(
                    response.Trim(),
                    ignoreCase: true,
                    out var intent))
            {
                return intent.ToString();
            }

            return IntentType.Unknown.ToString();
        }
    }
}
