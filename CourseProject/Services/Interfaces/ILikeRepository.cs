using CourseProject.Models;

namespace CourseProject.Services.Interfaces
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
        Task<Like?> GetByUserAndItem(string userId, Guid itemId);
        Task DeleteByUserAsync(string id);
    }
}
