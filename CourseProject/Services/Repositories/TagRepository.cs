using CourseProject.Models;
using CourseProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services.Repositories
{
    public class TagRepository : ITagRepository
    {
        private DbContextAsync _context;

        public TagRepository(DbContextAsync context)
        {
            _context = context;
        }

        public async Task CreateAsync(Tag entity)
        {
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<Tag> entities)
        {
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tag entity)
        {
            _context.Tags.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetAsync(Guid id)
        {
            return await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task UpdateAsync(Tag entity)
        {
            var item = await _context.Tags.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _context.Tags.Remove(item);
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
