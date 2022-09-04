using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class ErrorController : Controller
    {
        [Route ("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Sorry, we are unable to process your request";
                    break;
                default:
                    ViewBag.ErrorMessage = "Sorry, unexpected error";
                    break;
            }
            return View("Error");
        }
    }
}
