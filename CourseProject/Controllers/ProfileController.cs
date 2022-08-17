using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {

        public IActionResult Profile()
        {
            return View();
        }
    }
}
