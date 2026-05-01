namespace Construction.Models
{
    public class UploadAppendixRequest
    {
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int WorkId { get; set; }
    }
}
