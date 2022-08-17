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
        Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal);
        Task<string> GetUserIdAsync(ClaimsPrincipal principal);
        IdentityResult ErrorState { get; protected set; }
    }
}
