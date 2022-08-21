using CourseProject.Models;

namespace CourseProject.Services.Interfaces
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Task<Tag> GetByName(string name);
        Task CreateRangeAsync(IEnumerable<Tag> entities);
        Task<IEnumerable<Tag>> ExistsAsync(List<Tag> enities);
        Task<IEnumerable<Tag>> NotExistsAsync(IEnumerable<Tag> enities);
    }
}
