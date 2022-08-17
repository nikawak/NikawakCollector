﻿namespace CourseProject.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
        public List<Property> Properties { get; set; }
        
    }
}
