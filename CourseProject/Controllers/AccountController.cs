using CourseProject.Models;
using CourseProject.Models.ViewModels;
using CourseProject.Services;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CourseProject.Controllers
{
    public class AccountController : Controller
    {

        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            await Task.CompletedTask;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(CreateUserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterAsync(userVM);
                if (result?.Succeeded == true)
                {
                    return RedirectToAction("Profile", "Profile");
                }
                foreach(var error in _accountService.ErrorState.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(userVM);
        }
        [HttpGet]
        public async Task<IActionResult> Authorize()
        {
            await Task.CompletedTask;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorize(AuthorizeUserViewModel userVM)
        {
            if (ModelState.IsValid)
            {

                var result = await _accountService.AuthorizeAsync(userVM);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Profile");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Wrong Username or password");
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}