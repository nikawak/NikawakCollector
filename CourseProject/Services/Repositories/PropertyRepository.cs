using CourseProject.Models;
using CourseProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private DbContextAsync _context;

        public PropertyRepository(DbContextAsync context)
        {
            _context = context;
        }

        public async Task CreateAsync(Property entity)
        {
            await _context.Properties.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<Property> properties)
        {
            await _context.AddRangeAsync(properties);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Property entity)
        {
            _context.Properties.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties.ToListAsync();
        }

        public async Task<Property> GetAsync(Guid id)
        {
            return await _context.Properties.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Property>> GetByItemAsync(Guid itemId)
        {
            return await _context.Properties.Where(x => x.ItemId == itemId).ToListAsync();
        }

        public async Task UpdateAsync(Property entity)
        {
            var item = await _context.Properties.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _context.Properties.Remove(item);
            await _context.Properties.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
