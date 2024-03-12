using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using AzureDocumentSearch.Model.Response;
using Azure.Storage.Blobs;
using System.IO.Compression;
using AiHireService.Model;

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
        private static string blobStorageConnectionString;
        private static string blobContainerName;
        private static BlobContainerClient _blobContainerClient;


        public AiHireServiceLayer()
        {
            // Create a configuration using appsettings.json
            _builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _configuration = _builder.Build();

            // Read the values from appsettings.json
            searchServiceUri = _configuration["SearchServiceUri"];
            queryApiKey = _configuration["SearchServiceQueryApiKey"];
            index = _configuration["SearchServiceIndex"];
            blobStorageConnectionString = _configuration["StorageConnectionString"];
            blobContainerName = _configuration["StorageContainerName"];
            _blobContainerClient = new BlobContainerClient(
       blobStorageConnectionString,
        blobContainerName);
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

        public async Task<BlobFile> Download(string blobName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
            var props = blobClient.GetProperties();
            var stream =  await blobClient.OpenReadAsync();
            return new BlobFile()
            {
                Content = stream,
                ContentType = props.Value.ContentType,
            };
        }
        public async Task<List<BlobFile>> DownloadAll()
        {
            var blobs = new List<BlobFile>();

            await foreach (var blob in _blobContainerClient.GetBlobsAsync())
            {
                BlobClient blobClient = _blobContainerClient.GetBlobClient(blob.Name);
                var props = blobClient.GetProperties();
                var stream = await blobClient.OpenReadAsync();
                blobs.Add(new BlobFile()
                {
                    Content = stream,
                    ContentType = props.Value.ContentType,
                    Name = blob.Name
                });
            }
            return blobs;
        }

        public async Task Delete(string blobName)
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task DeleteAll()
        {
            await foreach (var blob in _blobContainerClient.GetBlobsAsync())
            {
                BlobClient blobClient = _blobContainerClient.GetBlobClient(blob.Name);
                await blobClient.DeleteIfExistsAsync();
            }
        }
    }
}
