using AiHireService.Service;
using Microsoft.AspNetCore.Mvc;

namespace AzureDocumentSearch.Controllers
{
    [ApiController]
    [Route("/")]
    public class AiHireController : ControllerBase
    {        
        private readonly ILogger<AiHireController> _logger;
        private readonly IAiHireServiceLayer _service;

        public AiHireController(ILogger<AiHireController> logger, IAiHireServiceLayer service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet(Name = "Search")]
        public IActionResult Search(string searchData)
        {
            return Ok(_service.SearchService(searchData));
        }


        //private async Task<ActionResult> RunQueryAsync(SearchData model)
        //{
        //    var options = new SearchOptions()
        //    {
        //        IncludeTotalCount = true
        //    };

        //    // Enter Hotel property names to specify which fields are returned.
        //    // If Select is empty, all "retrievable" fields are returned.
        //    options.Select.Add("HotelName");
        //    options.Select.Add("Category");
        //    options.Select.Add("Rating");
        //    options.Select.Add("Tags");
        //    options.Select.Add("Address/City");
        //    options.Select.Add("Address/StateProvince");
        //    options.Select.Add("Description");

        //    // For efficiency, the search call should be asynchronous, so use SearchAsync rather than Search.
        //    model.resultList = await _searchClient.SearchAsync<Hotel>(model.searchText, options).ConfigureAwait(false);

        //    // Display the results.
        //    return View("Index", model);
        //}
    }
}