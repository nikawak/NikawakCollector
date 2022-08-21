using Microsoft.AspNetCore.Identity;

namespace CourseProject.Models
{
    public class Like
    {
        public Guid Id { get; set; }
      
        public string SenderId { get; set; }
        public IdentityUser Sender { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; }
    }
}
