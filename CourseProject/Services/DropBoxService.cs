using Dropbox.Api;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Services
{
    public static class DropBoxService
    {
        private static string _accessToken = "sl.BNqQAI1p35WCUWzTdTUiQqm7OgLGg5uqAI_Pf_w_sPOwVsyonRmPVZAIDmjkdKIK-k8y5uqFFwkGVruktG1ECBrdYzabEC7chPFpPwsxuwuO98xtrBOUjdblr6Vm-fmU6mKeIRqayDcY";

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
