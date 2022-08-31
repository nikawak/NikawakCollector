using CourseProject.Models;
using CourseProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services.Repositories
{
    public class CollectionPropertyRepository : ICollectionPropertyRepository
    {
        private DbContextAsync _context;

        public CollectionPropertyRepository(DbContextAsync context)
        {
            _context = context;
        }

        public async Task CreateAsync(CollectionProperty entity)
        {
            await _context.CollectionProperties.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task CreateRangeAsync(IEnumerable<CollectionProperty> entity)
        {
            await _context.CollectionProperties.AddRangeAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CollectionProperty entity)
        {
            _context.CollectionProperties.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CollectionProperty>> GetAllAsync()
        {
            return await _context.CollectionProperties.ToListAsync();
        }

        public async Task<CollectionProperty> GetAsync(Guid id)
        {
            return await _context.CollectionProperties.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<CollectionProperty>> GetByCollectionAsync(Guid collectionId)
        {
            return await _context.CollectionProperties.Where(x => x.CollectionId == collectionId).ToListAsync();
        }
        public async Task DeleteRangeAsync(IEnumerable<CollectionProperty> entities)
        {
            _context.CollectionProperties.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(CollectionProperty entity)
        {
            await Task.CompletedTask;
            _context.CollectionProperties.Update(entity);
        }
    }
}
