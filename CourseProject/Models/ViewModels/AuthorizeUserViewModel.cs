using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models.ViewModels
{
    public class AuthorizeUserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password must be 5+ symbols")]
        public string Password { get; set; }


    }
}