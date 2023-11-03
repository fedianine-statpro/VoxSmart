using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace VoxSmart.Test
{
    public class ConfigurationLoadingTests
    {
        /// <summary>
        /// This test verifies that the configuration settings are loaded correctly.
        /// </summary>
        [Fact]
        public void ConfigurationShouldLoadAppSettings()
        {
            var host = Program.CreateHostBuilder(new string[] { }).Build();
            var config = host.Services.GetRequiredService<IConfiguration>();

            var feedUrl = config["AppSettings:FeedUrl"];
            var nlpFilteringEntities = config.GetSection("AppSettings:NLPFilteringEntities").Get<string[]>();

            Assert.False(string.IsNullOrWhiteSpace(feedUrl));
            Assert.NotNull(nlpFilteringEntities);
            Assert.NotEmpty(nlpFilteringEntities);
        }
    }
}
