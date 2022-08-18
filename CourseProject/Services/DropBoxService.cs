using Dropbox.Api;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Services
{
    public static class DropBoxService
    {
        private static string _accessToken = "sl.BNnQTBzRjOmRnaSvaSii6Ecw7tSHRYAz8pF11lOS8GMd6S3OL0pRZlk7GFzgeDRdybiQiAn1ubutSaIYKvRXa4-ep0rD5O9jkRLkuzwgPQepSGfwo5ojQWISUdrcbD9OLCy9ElJzFq7B";

        public static async Task UploadImageAsync(IFormFile image, string imageName)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                var response = await dbx.Files.UploadAsync(imageName, body: image.OpenReadStream());
            }
        }

        public static async Task<byte[]> ReadImageAsync(string imageName)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                using (var response = await dbx.Files.DownloadAsync(imageName))
                {
                    var bytes = await response.GetContentAsByteArrayAsync();
                    return bytes;
                }
            }
        }
    }
}
