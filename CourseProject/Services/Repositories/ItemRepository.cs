﻿using CourseProject.Models;
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
            return await _context.Items
                .Include(t => t.Tags).Include(p => p.Properties)
                .Include(l => l.Likes).Include(c => c.Comments)
                .ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            return await _context.Items
                .Include(t => t.Tags).Include(p => p.Properties)
                .Include(l => l.Likes).Include(c => c.Comments)
                    .ThenInclude(u=>u.Sender)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Item>> GetByCollectionAsync(Guid collectionId)
        {
            return await _context.Items
                .Where(x => x.CollectionId == collectionId)
                .Include(t => t.Tags).Include(p => p.Properties)
                .Include(l => l.Likes).Include(c => c.Comments)
                .Where(x => x.IsPrivate == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetByCollectionWithPrivateAsync(Guid collectionId)
        {
            return await _context.Items
                .Where(x => x.CollectionId == collectionId)
                .Include(t => t.Tags).Include(p => p.Properties)
                .Include(l => l.Likes).Include(c => c.Comments)
                .ToListAsync();
        }

        public async Task UpdateAsync(Item entity)
        {
            await Task.CompletedTask;
            _context.Items.Update(entity);
        }
    }
}
