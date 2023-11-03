using Microsoft.Extensions.Logging;
using Moq;
using VoxSmart.Implementations;
using Xunit;

namespace VoxSmart.Test
{
    public class FeedParserUrlValidationTests
    {
        /// <summary>
        /// This test ensures that an ArgumentNullException is thrown when an empty or null URL is passed.
        /// </summary>
        [Fact]
        public async Task ExtractFinancialEntitiesWithNullOrEmptyUrlShouldThrowArgumentNullException()
        {
            var mockLogger = new Mock<ILogger<DowJonesFeedParser>>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockNlpPipeline = new Mock<NlpPipeline>();
            var parser = new DowJonesFeedParser(mockLogger.Object, mockHttpClient.Object, mockNlpPipeline.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => parser.ExtractFinancialEntitiesAsync(null, new string[] { }, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => parser.ExtractFinancialEntitiesAsync("", new string[] { }, null));
        }
    }
}
