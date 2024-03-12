namespace AiHireService.Model
{
    public class BlobFile
    {
        public Stream Content { get; set; }
        public string ContentType { get; set; }

        public string? Name { get; set; }
    }
}
