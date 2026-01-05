using System.Text.Json;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent
{
    public class LlmAgentDecisionService : IAgentDecisionService
    {
        private readonly ILlmClient _llmClient;

        public LlmAgentDecisionService(ILlmClient llmClient)
        {
            _llmClient = llmClient;
        }

        public async Task<AgentDecision> CreateDecisionAsync(string userInput)
        {
            var systemPrompt = BuildSystemPrompt();
            var userPrompt = BuildUserPrompt(userInput);

            string rawResponse =
                await _llmClient.GetCompletionAsync(systemPrompt, userPrompt);

            return ParseDecision(rawResponse);
        }

        private static string BuildSystemPrompt()
        {
            return """
            You are a decision engine for an AI agent.

            Your job is NOT to perform tasks.
            Your job is to decide which tool (if any) should be used.

            You must output ONLY valid JSON matching this schema:

            {
              "toolName": string | null,
              "toolInput": string | null,
              "confidence": number (0.0 - 1.0),
              "requiresHumanReview": boolean,
              "reasoning": string
            }

            Rules:
            - If the request is ambiguous, set requiresHumanReview = true
            - If no tool applies, set toolName = null
            - Do not include explanations outside JSON
            - Be conservative with confidence
            """;
        }

        private static string BuildUserPrompt(string input)
        {
            return $"""
            User request:
            {input}

            Decide the best next action.
            """;
        }

        private static AgentDecision ParseDecision(string rawJson)
        {
            try
            {
                var decision = JsonSerializer.Deserialize<AgentDecision>(
                    rawJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (decision == null)
                    throw new InvalidOperationException("Decision was null.");

                return decision;
            }
            catch (Exception ex)
            {
                // Fail safe: require human review if parsing fails
                return new AgentDecision
                {
                    RequiresHumanReview = true,
                    Confidence = 0.0,
                    Reasoning =
                        $"Failed to parse LLM decision. Raw output: {rawJson}. Error: {ex.Message}",
                    Exception = ex
                };
            }
        }
    }
}
