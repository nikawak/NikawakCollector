using CourseProject.Models.ViewModels;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseProject.Controllers
{
    public class AccountController : Controller
    {

        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
                    return RedirectToAction("Main", "Profile");
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
                    return RedirectToAction("Main", "Profile");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Wrong Username or password");
                }
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Authorize");
        }

    }
}