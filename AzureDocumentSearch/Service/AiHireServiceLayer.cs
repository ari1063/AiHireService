using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using AzureDocumentSearch.Model.Response;

namespace AiHireService.Service
{
    public class AiHireServiceLayer : IAiHireServiceLayer
    {
        private static SearchClient _searchClient;
        private static SearchIndexClient _indexClient;
        private static IConfigurationBuilder _builder;
        private static IConfigurationRoot _configuration;
        private static string searchServiceUri;
        private static string queryApiKey;
        private static string index;

        public AiHireServiceLayer()
        {
            // Create a configuration using appsettings.json
            _builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _configuration = _builder.Build();

            // Read the values from appsettings.json
            searchServiceUri = _configuration["SearchServiceUri"];
            queryApiKey = _configuration["SearchServiceQueryApiKey"];
            index = _configuration["SearchServiceIndex"];
        }

        public List<SearchResponse> SearchService(string searchData)
        {
            List<SearchResponse> ret = new List<SearchResponse>();
            // Create a service and index client.
            _indexClient = new SearchIndexClient(new Uri(searchServiceUri), new AzureKeyCredential(queryApiKey));

            _searchClient = _indexClient.GetSearchClient(index);
            var response = _searchClient.Search<SearchResponse>(searchData).Value;
            foreach (Azure.Search.Documents.Models.SearchResult<SearchResponse> result in response.GetResults())
            {
                ret.Add(result.Document);
            }

            return ret;
        }
    }
}
