using CourseProject.Models;

namespace CourseProject.Services.Interfaces
{
    public interface IPropertyRepository : IBaseRepository<Property>
    {
        Task<IEnumerable<Property>> GetByItemAsync(Guid id);
        Task CreateRangeAsync(IEnumerable<Property> properties);
    }
}