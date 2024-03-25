namespace Collectium.Model.Bean.Request
{
    public class UploadRequestBean
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public int? CallId { get; set; }

        public IFormFile? File { get; set; }

    }
}
