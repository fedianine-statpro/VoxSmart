using Catalyst;
using Microsoft.Extensions.Logging;
using Mosaik.Core;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using VoxSmart.Interfaces;

namespace VoxSmart.Implementations
{
    /// <summary>
    /// Provides functionality for extracting financial entities from a given URL pointing to a Dow Jones news feed.
    /// </summary>
    public class DowJonesFeedParser : IFeedParser
    {
        private readonly ILogger<DowJonesFeedParser> _logger;
        private readonly HttpClient _httpClient;
        private readonly NlpPipeline _nlp;

        /// <summary>
        /// Constructs the Dow Jones Feed Parser with specified logger and HTTP client.
        /// </summary>
        /// <param name="logger">Logger for capturing runtime information and errors.</param>
        /// <param name="httpClient">HTTP client for sending requests to the news feed URL.</param>
        /// <param name="nlp">Natural language processing pipeline.</param>
        public DowJonesFeedParser(ILogger<DowJonesFeedParser> logger, HttpClient httpClient, NlpPipeline nlp)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _nlp = nlp;
        }

        /// <summary>
        /// Asynchronously extracts financial entities from the news content at the specified URL using NLP and Regex as defined by the provided filtering parameters.
        /// </summary>
        /// <param name="url">The URL of the news feed to parse.</param>
        /// <param name="nlpFilteringEntities">An array of entities used for filtering with NLP.</param>
        /// <param name="filteringRegex">The regex pattern used for extracting entities from the text.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of extracted financial entities.</returns>
        public async Task<IEnumerable<string>> ExtractFinancialEntitiesAsync(string? url, string[]? nlpFilteringEntities,
            string? filteringRegex)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            try
            {
                // Note: We are assuming that string is not too large to be loaded in memory
                string response = await _httpClient.GetStringAsync(url);

                // If there is no data in the response, assume there are no news available
                if (string.IsNullOrEmpty(response))
                    return Array.Empty<string>();

                // If there is no data in the provided xml, assume there are no news available
                var data = GetTextFromXml(response);
                if (string.IsNullOrEmpty(data))
                    return Array.Empty<string>();

                // Extract the financial entities using Natural Language Processing and Regex
                var financialEntitiesByNlp = ExtractFinancialEntitiesByNlp(data, nlpFilteringEntities);
                var financialEntitiesByRegex = ExtractFinancialEntitiesByRegex(data, filteringRegex);

                // Combine and remove duplicates using LINQ's Union extension method
                return financialEntitiesByNlp.Union(financialEntitiesByRegex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while extracting financial entities.");
                throw;
            }
        }

        /// <summary>
        /// Extracts and decodes combined text from the XML content, specifically targeting item titles and descriptions.
        /// </summary>
        /// <param name="xml">The XML content as a string from which to extract text.</param>
        /// <returns>The combined and decoded text from the XML.</returns>
        private string GetTextFromXml(string? xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentNullException(nameof(xml));

            var doc = XDocument.Parse(xml);

            // Combine the item titles and descriptions into a single string
            // Note: We are assuming that XML is always well-formed
            string combinedText = string.Join(" ", doc.Descendants("item")
                .Select(item => $"{item.Element("title")?.Value} {item.Element("description")?.Value}"));

            // Decode the HTML entities once for the whole combined text
            return WebUtility.HtmlDecode(combinedText);
        }

        /// <summary>
        /// Extracts financial entities from provided text data using a Regex pattern.
        /// </summary>
        /// <param name="data">The text data to search for financial entities.</param>
        /// <param name="filteringRegex">The Regex pattern to identify financial entities within the text.</param>
        /// <returns>A collection of financial entities identified by Regex.</returns>
        private IEnumerable<string> ExtractFinancialEntitiesByRegex(string data, string? filteringRegex)
        {
            if (string.IsNullOrEmpty(filteringRegex))
                return Array.Empty<string>();

            var matches = Regex.Matches(data, filteringRegex, RegexOptions.Compiled);
            return matches.Where(m => m.Success).Select(m => m.Value);
        }

        /// <summary>
        /// Asynchronously extracts financial entities from provided text data using NLP based on specified filtering entities.
        /// </summary>
        /// <param name="data">The text data to process with NLP for entity extraction.</param>
        /// <param name="nlpFilteringEntities">An array of entity types to use for NLP filtering.</param>
        /// <returns>A task that represents the asynchronous operation of NLP processing. The task result contains a collection of financial entities extracted.</returns>
        private IEnumerable<string> ExtractFinancialEntitiesByNlp(string data, string[]? nlpFilteringEntities)
        {
            if (nlpFilteringEntities == null || !nlpFilteringEntities.Any())
                return Array.Empty<string>();

            var doc = new Document(data, Language.English);
            _nlp.Pipeline.ProcessSingle(doc);
            return from span in doc
                   from entity in span.GetEntities(entityType => nlpFilteringEntities.Contains(entityType.Type, StringComparer.InvariantCultureIgnoreCase))
                   select entity.Value;
        }
    }
}
