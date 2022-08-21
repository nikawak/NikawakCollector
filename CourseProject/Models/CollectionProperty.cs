using CourseProject.Models.Enums;

namespace CourseProject.Models
{
    public class CollectionProperty
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public PropertyType Type { get; set; }

        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }

        public List<Property> Properties { get; set; }
    }
}
