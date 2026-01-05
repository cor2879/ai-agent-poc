using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OldSkoolGamesAndSoftware.AiAgent.Orchestrator.Agent;

namespace OldSkoolGamesAndSoftware.AiAgent.Orchestrator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    var startup = new Startup(context.Configuration);
                    startup.ConfigureServices(services);
                })
                .Build();

            using var scope = host.Services.CreateScope();
            var agent = scope.ServiceProvider.GetRequiredService<AgentEngine>();

            var result = await agent.HandleAsync(
                "Please summarize this incident report");

            Console.WriteLine("Final Result:");
            Console.WriteLine(result.Output);
        }
    }
}