using CourseProject.Models;
using CourseProject.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    public class DbContextAsync : IdentityDbContext
    {
        public DbContextAsync(DbContextOptions options) : base(options) { }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionProperty> CollectionProperties { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach(var foreignKey in builder.Model.GetEntityTypes()
                                                   .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.ClientCascade;
            }
            builder.Entity<CollectionProperty>()
                .Property(e => e.Type)
                .HasConversion(
                v => v.ToString(),
                v => (PropertyType)Enum.Parse(typeof(PropertyType), v));

            builder.Entity<Collection>()
                .Property(e => e.Theme)
                .HasConversion(
                v => v.ToString(),
                v => (CollectionTheme)Enum.Parse(typeof(CollectionTheme), v));
        }


    }
}
