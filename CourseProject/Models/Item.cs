namespace CourseProject.Models
{
    public class Item
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public DateTime CreatingDate { get; set; }
        public bool IsPrivate { get; set; }

        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }

        public List<Property> Properties { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
    }
}
