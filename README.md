

# Financial Entity Extractor

This application is a simple console-based utility written in C# to extract financial entity names from an online feed. It leverages a natural language processing pipeline to identify financial entities such as currencies, securities, and company names from `https://feeds.a.dj.com/rss/RSSMarketsMain.xml`.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- .NET 6.0 SDK or later
- An internet connection to access the feed and to download NLP model data

### Installing

1. Clone the repository to your local machine.
2. Navigate to the cloned directory.
3. Run the following command to restore the dependencies:

   `dotnet restore`

   Build the application:

   `dotnet build`
   
### Running the Application

Execute the application with the following command:

   `dotnet run`

The application will automatically download the necessary NLP model data upon the first run.

## Usage

### Extensibility

The application is designed to be extensible:

    New financial entities can be added to the NLPFilteringEntities array in the appsettings.json.
    The regular expression used for matching can be adjusted via the FilteringRegex setting in appsettings.json.

### Questions and Assumptions
Built With

   .NET 6.0 - The framework used
    Catalyst - NLP Library for .NET

   **XML Data Format and Structure**: We're assuming that the data will always be in XML format, will always be structured in the way we expect, and won't have any errors in the formatting.

   **Data Size**: We believe that the data will always be small enough to fit into the computer's memory without any trouble.

   **Scope of Parsing**: We're assuming that just showing how I can parse company names with NLP and currencies with RegEx is sufficient to demonstrate my expertise.

**Language Assumption**: We're assuming that the NLP will only need to understand English for this exercise.

**Use of Pre-existing Libraries**: Another option could have been to use a ready-made list of all financial entities and filter through them, but we assume for this exercise, building our process was necessary.

   **Load and Scalability**: We're assuming that this code will not face heavy usage, so making it scalable was not considered important for now.

   **Security Against ReDoS**: We are not protecting against ReDoS (Regular Expression Denial of Service) attacks; assuming it's not a risk for this exercise.

   **Feed Content Structure**: Assumed that the RSS feed at https://feeds.a.dj.com/rss/RSSMarketsMain.xml maintains a consistent XML structure, and that important information is contained within the <item> elements.

   **XML Data Format**: Assumed that the XML data from the feed is always well-formed and does not contain malformed tags or invalid characters which could break the XML parser.

   **Feed Availability**: Assumed that the feed is available and accessible at all times without the need for authentication or special access permissions.

   **Financial Entity Definition**: Assumed a financial entity to be any named entity that can be traded, such as a currency, security, or company name. The program does not account for more abstract financial instruments unless explicitly defined in the NLPFilteringEntities.

   **Natural Language Processing (NLP) Completeness**: Assumed that the NLP model provided is capable of identifying the entities of interest with reasonable accuracy, though it may not be exhaustive in its identification of all possible financial entities.

   **Regular Expression Coverage**: Assumed that the regular expression defined in the appsettings.json for FilteringRegex is sufficient for identifying additional financial entities not caught by the NLP.

   **Feed Update Frequency**: Assumed that the feed updates at a frequency that does not require real-time monitoring, allowing the application to be run periodically (e.g., daily) rather than continuously.

   **Application Environment**: Assumed that the application runs in an environment with stable internet access and sufficient permissions to access network resources and write to the local filesystem (for model data storage).

   **Performance and Scalability**: Assumed that the current scale of data and frequency of execution do not necessitate a distributed or highly optimized computing solution.

   **Data Privacy and Security**: Assumed that the data in the feed is public information and does not require special considerations for privacy or security beyond standard best practices.

   **Error Handling**: Assumed that basic error handling is sufficient for the scope of the exercise, and extensive fault tolerance and recovery mechanisms are not required.

   **Unicode and Character Encoding**: Assumed that all text in the XML feed is encoded in UTF-8 and does not contain encoding errors that could cause issues in processing.

   **Continuous Deployment/Delivery**: Assumed that a CI/CD pipeline is not necessary for the exercise and that manual deployment is sufficient for testing and demonstration purposes.

   **Maintenance and Support**: Assumed that the application does not require a formal support and maintenance plan, given its use as an exercise demonstration.

   **External Dependencies**: Assumed that third-party services and libraries used (e.g., Catalyst for NLP) are reliable and remain functional and available throughout the lifecycle of this application.

#### Authors
Victor Fedianine