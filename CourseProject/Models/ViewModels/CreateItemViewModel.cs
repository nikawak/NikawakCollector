namespace CourseProject.Models.ViewModels
{
    public class CreateItemViewModel
    {
        public string Name { get; set; }
        public string Tags { get; set; }

        public Guid CollectionId { get; set; }
    }
}