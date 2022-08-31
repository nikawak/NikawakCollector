using CG.Web.MegaApiClient;

namespace CourseProject.Services
{
    public static class MegaService
    {
        //private static string _megaKey = "ViwcvMyGCL88pOyBC8ZGFg";

        public static async Task<string> UploadImageAsync(IFormFile image, string imageName, IConfiguration configuration)
        {
            MegaApiClient client = new MegaApiClient();
            client.Login(configuration["MegaService:MegaLogin"], configuration["MegaService:MegaPassword"]);

            IEnumerable<INode> nodes = client.GetNodes();

            INode folder = nodes.Single(x => x.Name == "nikawak");

            INode file = await client.UploadAsync(image.OpenReadStream(), imageName, folder);
            Uri downloadLink = client.GetDownloadLink(file);
            var path = downloadLink.OriginalString;
            client.Logout();

            return path;
        }

        internal static async Task<byte[]> ReadImageAsync(string imageName)
        {
            MegaApiClient client = new MegaApiClient();
            client.LoginAnonymous();

            Uri fileLink = new Uri(imageName);
            INode node = client.GetNodeFromLink(fileLink);

            var stream = await client.DownloadAsync(node);
            var bytes = new byte[stream.Length];

            stream.Read(bytes);

            client.Logout();

            return bytes;
        }
  
    }
}
