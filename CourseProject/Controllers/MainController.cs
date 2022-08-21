using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class MainController : Controller
    {
        private ISeedData _seedData;

        public MainController(ISeedData seedData)
        {
            _seedData = seedData;
        }
        public async Task<IActionResult> Index()
        {
            //await _seedData.EnsureData();
            return View();
        }
    }
}
