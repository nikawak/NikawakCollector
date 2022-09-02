using CourseProject.Models;
using CourseProject.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CourseProject.Services.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> RegisterAsync(CreateUserViewModel userVM);
        Task<SignInResult> AuthorizeAsync(AuthorizeUserViewModel userVM);
        Task LogoutAsync();
        Task<User> GetUserAsync(ClaimsPrincipal userPrincipal);
        IEnumerable<User> GetUsers();
        Task<IEnumerable<string>> GetRolesAsync(User user);
        Task<string> GetUserIdAsync(ClaimsPrincipal userPrincipal);

        Task BlockUserAsync(string userId);
        Task UnblockUserAsync(string userId);
        Task DeleteUserAsync(string userId);
        Task AddToAdminAsync(string userId);
        Task DeleteFromAdminAsync(string userId);
        IdentityResult ErrorState { get; protected set; }
    }
}
