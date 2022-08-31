namespace CourseProject.Services.Interfaces
{
    public interface IBaseRepository <T>
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T collection);
    }
}
