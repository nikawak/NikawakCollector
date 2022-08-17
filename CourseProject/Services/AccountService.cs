using CourseProject.Models;
using CourseProject.Models.ViewModels;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CourseProject.Services
{
    public class AccountService : IAccountService
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public IdentityResult ErrorState { get; set; }

        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<SignInResult> AuthorizeAsync(AuthorizeUserViewModel userVM)
        {
            var user = await _userManager.FindByNameAsync(userVM.UserName);

            SignInResult signInResult = null;

            signInResult = await _signInManager.PasswordSignInAsync(user, userVM.Password, false, false);

            return signInResult;
        }

        public async Task<SignInResult> RegisterAsync(CreateUserViewModel userVM)
        {
            var user = new IdentityUser()
            {
                UserName = userVM.UserName,
                Email = userVM.Email,
            };

            var resultUser = await _userManager.CreateAsync(user, userVM.Password);
            var resultRole = await _userManager.AddToRoleAsync(user, "User");

            SignInResult signInResult = null;
            if (resultUser.Succeeded && resultRole.Succeeded)
            {
                signInResult = await _signInManager.PasswordSignInAsync(user, userVM.Password, false, false);
            }
            ErrorState = resultUser;
            return signInResult;
        }

        public async Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<string> GetUserIdAsync(ClaimsPrincipal principal)
        {
            var user = await GetUserAsync(principal);
            var id = await _userManager.GetUserIdAsync(user);
            return id;
        }

    }
}
