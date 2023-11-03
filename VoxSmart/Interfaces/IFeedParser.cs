namespace VoxSmart.Interfaces
{
    public interface IFeedParser
    {
        /// <summary>
        /// Asynchronously extracts financial entities from the news content at the specified URL using NLP and Regex as defined by the provided filtering parameters.
        /// </summary>
        /// <param name="url">The URL of the news feed to parse.</param>
        /// <param name="nlpFilteringEntities">An array of entities used for filtering with NLP.</param>
        /// <param name="filteringRegex">The regex pattern used for extracting entities from the text.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of extracted financial entities.</returns>

        Task<IEnumerable<string>> ExtractFinancialEntitiesAsync(string? url, string[]? nlpFilteringEntities,
            string? filteringRegex);
    }
}
