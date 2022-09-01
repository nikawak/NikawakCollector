using CourseProject.Models;
using CourseProject.Models.ViewModels;
using CourseProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseProject.Services
{
    public class AccountService : IAccountService
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private IUnitOfWork _unitOfWork;

        public IdentityResult ErrorState { get; set; }

        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }


        public async Task<SignInResult> AuthorizeAsync(AuthorizeUserViewModel userVM)
        {
            var user = await _userManager.FindByNameAsync(userVM.UserName);

            SignInResult signInResult = null;

            signInResult = await _signInManager.PasswordSignInAsync(user, userVM.Password, false, false);

            if (!await _userManager.IsInRoleAsync(user, "User"))
            {
                await LogoutAsync();
            }
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

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);
            await _unitOfWork.CollectionRepository.DeleteByUserAsync(id);
            await _unitOfWork.CommentRepository.DeleteByUserAsync(id);
            await _unitOfWork.LikeRepository.DeleteByUserAsync(id);

            await _userManager.DeleteAsync(user);
        }
        public async Task AddToAdminAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        public async Task DeleteFromAdminAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "Admin");
        }
        public async Task BlockUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);
        }
        public async Task UnblockUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.AddToRoleAsync(user, "User");
        }

        public IEnumerable<IdentityUser> GetUsers()
        {
            return _userManager.Users;
        }
    }
}
