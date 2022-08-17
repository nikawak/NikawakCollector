using CourseProject.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models.ViewModels
{
    public class CreateCollectionViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Description="Collection Theme")]
        public CollectionTheme Theme { get; set; }
        [Display(Description = "Item Property")]
        public PropertyType PropertyType { get; set; }

        public IFormFile Image { get; set; }

    }
}