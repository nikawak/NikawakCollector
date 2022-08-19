using Dropbox.Api;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Services
{
    public static class DropBoxService
    {
        private static string _accessToken = "sl.BNpxDHltEigLTAQoneuv6SoE3LFGLnPdQ2zFhNGHwCVtLrQX5EGXlVcFBxL0MxvkgBqW-JEUQLhO-jJlUvKs5O8mk5mTrp_15hKj3TnHIjKf8vkhfUILN1YkXusQIIRHwQHD_bsuA4rl";

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
