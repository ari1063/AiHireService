using AiHireService.Model;
using AzureDocumentSearch.Model.Response;

namespace AiHireService.Service
{
    public interface IAiHireServiceLayer
    {
        List<SearchResponse> SearchService(string searchData);
        Task<BlobFile> Download(string blobName);
        Task<List<BlobFile>> DownloadAll();

        Task Delete(string blobName);
        Task DeleteAll();
    }
}
