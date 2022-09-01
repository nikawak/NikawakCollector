using CourseProject.Models;
using CourseProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private DbContextAsync _context;

        public LikeRepository(DbContextAsync context)
        {
            _context = context;
        }

        public async Task CreateAsync(Like entity)
        {
            await _context.Likes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Like entity)
        {
            _context.Likes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Like>> GetAllAsync()
        {
            return await _context.Likes.ToListAsync();
        }

        public async Task<Like> GetAsync(Guid id)
        {
            return await _context.Likes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Like?> GetByUserAndItem(string userId, Guid itemId)
        {
            return await _context.Likes.FirstOrDefaultAsync(x => x.SenderId == userId && x.ItemId == itemId);
        }

        public async Task UpdateAsync(Like entity)
        {
            _context.Likes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByUserAsync(string id)
        {
            var likes = _context.Likes
                .Where(u => u.SenderId == id).ToList();

            _context.Likes.RemoveRange(likes);
            await _context.SaveChangesAsync();
        }
    }
}
