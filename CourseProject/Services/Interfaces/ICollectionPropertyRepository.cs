using CourseProject.Models;

namespace CourseProject.Services.Interfaces
{
    public interface ICollectionPropertyRepository : IBaseRepository<CollectionProperty>
    {
        Task CreateRangeAsync(IEnumerable<CollectionProperty> entity);
        Task<IEnumerable<CollectionProperty>> GetByCollectionAsync(Guid collectionId);
    }
}
