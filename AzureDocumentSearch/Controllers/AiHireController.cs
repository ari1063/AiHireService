using AiHireService.Model;
using AiHireService.Service;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

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

        #region Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var response = await _service.UploadFiles(files);
            return Ok(response);
        }
        #endregion

        #region Search
        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string searchData)
        {
            return Ok(_service.SearchService(searchData));
        }
        #endregion

        #region Download

        [HttpPost]
        [Route("Download")]
        public  FileResult Download(string blobName)
        {
            BlobFile blob = Task.Run(() => _service.Download(blobName)).Result;
            var contentType = "APPLICATION/octet-stream";
            return File(blob.Content, contentType, blobName);
        }

        [HttpPost]
        [Route("DownloadAll")]
        public FileResult DownloadAll()
        {
            List<BlobFile> blobList = Task.Run(() => _service.DownloadAll()).Result;

            using (MemoryStream ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var file in blobList)
                    {
                        var entry = archive.CreateEntry(file.Name, CompressionLevel.Fastest);
                        using (var zipStream = entry.Open())
                        {
                            using (MemoryStream msc = new MemoryStream())
                            {
                                file.Content.CopyTo(msc);
                                var content = msc.ToArray();
                                zipStream.Write(content, 0, content.Length);
                                msc.SetLength(0);
                                msc.Position = 0;
                            }
                            
                        }
                    }
                }

                return File(ms.ToArray(), "application/zip", "Resumes.zip");
            }
        }

        #endregion

        #region Delete

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(string blobName)
        {
            return Ok(Task.Run(() => _service.Delete(blobName)));
        }

        [HttpDelete]
        [Route("DeleteAll")]
        public IActionResult DeleteAll()
        {
            {
                return Ok(Task.Run(() => _service.DeleteAll()));
            }
        }

        #endregion


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