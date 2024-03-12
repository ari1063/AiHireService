namespace AzureDocumentSearch.Model.Response
{
    public class SearchResponse
    {
        public string id { get; set; }
        public string content { get; set; }

        public string type { get; set; }
        public List<string> people { get; set; }
        public List<string> organizations { get; set; }
        public List<string> locations { get; set; }
        public string blobUrl { get; set; }
        public List<Data> data { get; set; }

        public string blobFileName { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public List<Entities> entities { get; set; }
    }

    public class Entities
    {
        public string text { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public long offset { get; set; }
        public long length { get; set; }
        public double confidenceScore { get; set; }
    }

    public class SearchDocumentResponseDTO
    {
        public List<string> people { get; set; }
        public List<string> organizations { get; set; }
        public List<string> locations { get; set; }
    }
}
