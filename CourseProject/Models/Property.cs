namespace CourseProject.Models
{
    public class Property
    {   public Guid Id { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; }

        public Guid CollectionPropertyId { get; set; }
        public CollectionProperty CollectionProperty { get; set; }

        public string PropertyValue { get; set; }
    }
}
