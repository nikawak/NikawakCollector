using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IAccountService _accountService;

        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<IActionResult> ManageUsers()
        {
            var users = _accountService.GetUsers();
            return View(users);
        }
        public async Task<IActionResult> Block(string[] userId)
        {
            foreach(var id in userId)
            {
                await _accountService.BlockUserAsync(id);
            }
            
            return RedirectToAction("ManageUsers");
        }


        public async Task<IActionResult> Unblock(string[] userId)
        {
            foreach (var id in userId)
            {
                await _accountService.UnblockUserAsync(id);
            }

            return RedirectToAction("ManageUsers");
        }


        public async Task<IActionResult> Delete(string[] userId)
        {
            foreach (var id in userId)
            {
                await _accountService.DeleteUserAsync(id);
            }

            return RedirectToAction("ManageUsers");
        }


        public async Task<IActionResult> AddToAdmin(string[] userId)
        {
            foreach (var id in userId)
            {
                await _accountService.AddToAdminAsync(id);
            }

            return RedirectToAction("ManageUsers");
        }


        public async Task<IActionResult> DeleteFromAdmin(string[] userId)
        {
            foreach (var id in userId)
            {
                await _accountService.DeleteFromAdminAsync(id);
            }

            return RedirectToAction("ManageUsers");
        }
    }
}
