using CourseProject.Models;

namespace CourseProject.Services.Interfaces
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Task CreateRangeAsync(IEnumerable<Tag> entities);
        Task<IEnumerable<Tag>> ExistsAsync(List<Tag> enities);
        Task<IEnumerable<Tag>> NotExistsAsync(IEnumerable<Tag> enities);
    }
}
