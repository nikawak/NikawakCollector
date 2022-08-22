using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models.ViewModels
{
    public class CreateItemViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Tags { get; set; }

        public bool IsPrivate { get; set; }

        public Guid CollectionId { get; set; }
    }
}