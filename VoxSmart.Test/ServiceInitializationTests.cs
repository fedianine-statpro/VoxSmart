using Microsoft.Extensions.DependencyInjection;
using VoxSmart.Implementations;
using VoxSmart.Interfaces;
using Xunit;

namespace VoxSmart.Test
{
    public class ServiceInitializationTests
    {
        /// <summary>
        /// This test checks whether the NlpPipeline and IFeedParser services are successfully initialized.
        /// </summary>
        [Fact]
        public async Task NlpPipelineAndFeedParserShouldInitialize()
        {
            var host = Program.CreateHostBuilder(new string[] { }).Build();
            var services = host.Services;

            var nlpPipeline = services.GetService<NlpPipeline>();
            await nlpPipeline.InitializeAsync();

            var parser = services.GetService<IFeedParser>();

            Assert.NotNull(nlpPipeline);
            Assert.NotNull(parser);
        }
    }
}
