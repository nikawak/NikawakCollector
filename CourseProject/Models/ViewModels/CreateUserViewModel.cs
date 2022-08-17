using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [MaxLength(24, ErrorMessage = "Your NickName so big")]
        [MinLength(3, ErrorMessage = "Your NickName so small")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter correct Email")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password must be 5+ symbols")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must be equals")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}