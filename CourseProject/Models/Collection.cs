using CourseProject.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace CourseProject.Models
{
    public class Collection
    {   public Guid Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CollectionTheme Theme { get; set; }

        public List<Item> CollectionItems { get; set; }
        public List<CollectionProperty> Properties { get; set; }
    }
}
