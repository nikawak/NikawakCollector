using Microsoft.AspNetCore.Identity;

namespace CourseProject.Models
{
    public class User : IdentityUser
    {
        public bool IsBlocked { get; set; }
    }
}
