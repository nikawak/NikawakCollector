using CourseProject.Models;
using CourseProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private DbContextAsync _context;

        public ItemRepository(DbContextAsync context)
        {
            _context = context;
        }

        public async Task CreateAsync(Item entity)
        {
            await _context.Items.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item entity)
        {
            _context.Items.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Item> entities)
        {
            _context.Items.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.Items.Include(t => t.Tags).Include(p => p.Properties).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            return await _context.Items.Include(t => t.Tags).Include(p=>p.Properties).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Item>> GetByCollectionAsync(Guid collectionId)
        {
            return await _context.Items.Where(x => x.CollectionId == collectionId).Include(t => t.Tags).Include(p => p.Properties).ToListAsync();
        }

        public async Task UpdateAsync(Item entity)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _context.Items.Remove(item);
            await _context.Items.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
