using AzureDocumentSearch.Model.Response;

namespace AiHireService.Service
{
    public interface IAiHireServiceLayer
    {
        List<SearchResponse> SearchService(string searchData);
    }
}
