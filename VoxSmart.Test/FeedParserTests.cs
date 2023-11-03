using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Moq.Protected;
using VoxSmart.Implementations;
using Xunit;

namespace VoxSmart.Test
{
    public class FeedParserTests
    {
        /// <summary>
        /// This test checks the parser's ability to extract financial entities with valid input data.
        /// </summary>
        [Fact]
        public async Task ExtractFinancialEntitiesWithValidDataShouldSucceed()
        {
            // Arrange
            var fakeResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("<xml><item><title>USD goes up</title><description>Apple stocks are down.</description></item></xml>")
            };

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(fakeResponseMessage)
                .Verifiable();
            
            var httpClient = new HttpClient(handlerMock.Object);

            var loggerMock = new Mock<ILogger<DowJonesFeedParser>>();
            var nlpPipelineMock = new Mock<NlpPipeline>();

            var parser = new DowJonesFeedParser(loggerMock.Object, httpClient, nlpPipelineMock.Object);

            // Act
            var entities = await parser.ExtractFinancialEntitiesAsync("http://fakeurl.com", new string[] { "Organisations" }, @"\bUSD\b");

            // Assert
            Assert.NotNull(entities);
            Assert.Contains("USD", entities);
        }
    }
}
