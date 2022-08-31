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

        public async Task<IEnumerable<Tag>> ExistsAsync(List<Tag> enities)
        {
            var all = await _context.Tags.ToListAsync();
            var res = new List<Tag>();
            for (int i = 0; i < all.Count; i++)
            {
                for(int j = 0; j<enities.Count(); j++)
                {
                    if (all[i].Name.Equals(enities[j].Name))
                    {
                        res.Add(all[i]);
                    }
                }
            }
            return res;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.Include(i => i.Items).ToListAsync();
        }

        public async Task<Tag> GetAsync(Guid id)
        {
            return await _context.Tags.Include(i => i.Items).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag> GetByName(string name)
        {
            return await _context.Tags
                .Include(i => i.Items)
                    .ThenInclude(p=>p.Properties)
                .Include(i=>i.Items)
                    .ThenInclude(l=>l.Likes)
                .Include(i => i.Items)
                    .ThenInclude(l => l.Comments)
                    .FirstOrDefaultAsync(n => n.Name == name);
        }

        public async Task<IEnumerable<Tag>> NotExistsAsync(IEnumerable<Tag> enities)
        {
            var all = await _context.Tags.ToListAsync();
            var res = new List<Tag>();
            foreach(var entity in enities)
            {
                if (!all.Select(x=>x.Name).Contains(entity.Name))
                {
                    res.Add(entity);
                }
            }
            return res;
        }
        public async Task UpdateAsync(Tag entity)
        {
            await Task.CompletedTask;
            _context.Tags.Update(entity);
        }
    }
}
