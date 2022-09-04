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
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IUnitOfWork _unitOfWork;

        public IdentityResult ErrorState { get; set; }

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }


        public async Task<SignInResult> AuthorizeAsync(AuthorizeUserViewModel userVM)
        {
            var user = await _userManager.FindByNameAsync(userVM.UserName);

            if (user.IsBlocked) return SignInResult.LockedOut;

            var signInResult = await _signInManager.PasswordSignInAsync(user, userVM.Password, false, false);

            return signInResult;
        }

        public async Task<SignInResult> RegisterAsync(CreateUserViewModel userVM)
        {
            var user = new User()
            {
                UserName = userVM.UserName,
                Email = userVM.Email,
                IsBlocked = false
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

        public async Task<User> GetUserAsync(ClaimsPrincipal principal)
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


            user.IsBlocked = true;
            await _userManager.UpdateAsync(user);
            await DeleteUserAsync(user.Id);
            await _userManager.CreateAsync(user);
        }
        public async Task UnblockUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            user.IsBlocked = false;
            await _userManager.UpdateAsync(user);
        }

        public IEnumerable<User> GetUsers()
        {
            return _userManager.Users.ToList();
        }
        public async Task<IEnumerable<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
