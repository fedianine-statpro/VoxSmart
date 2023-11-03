using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VoxSmart.Implementations;
using VoxSmart.Interfaces;

namespace VoxSmart
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var services = host.Services;
            var logger = services.GetRequiredService<ILogger<Program>>();
            var configuration = services.GetRequiredService<IConfiguration>();
            var nlpPipeline = services.GetRequiredService<NlpPipeline>();
            await nlpPipeline.InitializeAsync();
            var parser = services.GetRequiredService<IFeedParser>();

            try
            {
                var feedUrl = configuration["AppSettings:FeedUrl"];
                var nlpFilteringEntities = configuration.GetSection("AppSettings:NLPFilteringEntities").Get<string[]?>();
                string? filteringRegex = configuration["AppSettings:FilteringRegex"];
                var financialEntities = await parser.ExtractFinancialEntitiesAsync(feedUrl, nlpFilteringEntities, filteringRegex);

                // Process the extracted financial entities
                foreach (var entity in financialEntities)
                {
                    Console.WriteLine(entity);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while extracting financial entities.");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddSingleton<NlpPipeline>();
                    services.AddSingleton<IFeedParser, DowJonesFeedParser>();
                    services.AddLogging(configure => configure.AddConsole());
                });
    }
}