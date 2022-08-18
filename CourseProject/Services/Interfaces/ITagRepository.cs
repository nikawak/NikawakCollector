using CourseProject.Models;

namespace CourseProject.Services.Interfaces
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Task CreateRangeAsync(IEnumerable<Tag> entities);
    }
}
