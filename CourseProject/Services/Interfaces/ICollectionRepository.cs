using CourseProject.Models;

namespace CourseProject.Services.Interfaces
{
    public interface ICollectionRepository:IBaseRepository<Collection>
    {
        Task<IEnumerable<Collection>> GetByUserAsync(string userId);
    }
}
