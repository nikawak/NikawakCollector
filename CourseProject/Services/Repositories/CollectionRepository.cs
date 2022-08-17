﻿using CourseProject.Models;
using CourseProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private DbContextAsync _context;

        public CollectionRepository(DbContextAsync context)
        {
            _context = context;
        }

        public async Task CreateAsync(Collection entity)
        {
            await _context.Collections.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Collection entity)
        {
            _context.Collections.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Collection>> GetAllAsync()
        {
            return await _context.Collections.ToListAsync();
        }

        public async Task<Collection> GetAsync(Guid id)
        {
            return await _context.Collections.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Collection>> GetByUserAsync(string userId)
        {
            return await _context.Collections.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task UpdateAsync(Collection entity)
        {
            var item = await _context.Collections.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _context.Collections.Remove(item);
            await _context.Collections.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}