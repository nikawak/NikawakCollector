using CourseProject.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseProject.Helpers
{
    public static class EnumConverter
    {
        public static IEnumerable<SelectListItem> GetPropertyTypes()
        {
            return Enum.GetValues(typeof(PropertyType))
                .Cast<PropertyType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().Replace("_", " "),
                    Value = ((int)x).ToString()
                });
        }
        public static IEnumerable<SelectListItem> GetCollectionThemes()
        {
            return Enum.GetValues(typeof(CollectionTheme))
                .Cast<CollectionTheme>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = ((int)x).ToString()
                });
        }
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
