using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Infrastructure;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Telemetry;
using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // ---------- Configuration ----------
            services.Configure<OpenAiOptions>(
                _configuration.GetSection("OpenAI"));

            services.AddSingleton(sp =>
                sp.GetRequiredService<
                    Microsoft.Extensions.Options.IOptions<OpenAiOptions>>().Value);

            // ---------- HTTP ----------
            services.AddHttpClient<OpenAiHttpClient>();

            // ---------- OpenAI Clients ----------
            services.AddSingleton<ILlmClient, OpenAiLlmClient>();
            services.AddSingleton<ITextCompletionService, OpenAiTextCompletionService>();

            // ---------- Agent Core ----------
            services.AddSingleton<IAgentDecisionService, LlmAgentDecisionService>();
            services.AddSingleton<IToolRegistry, ToolRegistry>();
            services.AddSingleton<AgentEngine>();

            // ---------- Tools ----------
            services.AddSingleton<ITool, SummarizeTextTool>();
            services.AddSingleton<ITool, ClassifyIntentTool>();

            // ---------- Telemetry ----------
            services.AddSingleton<IAgentTelemetry, ConsoleAgentTelemetry>();
        }
    }
}
