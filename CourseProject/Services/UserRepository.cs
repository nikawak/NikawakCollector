using CourseProject.Models;
using CourseProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Services
{
    /*public class UserRepository : IUserRepository
    {
        private DbContextAsync _dbContext;
        public UserRepository(DbContextAsync dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(User entity)
        {
            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User entity)
        {
            _dbContext.Users.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User entity)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == entity.Id);
            _dbContext.Users.Remove(user);
            await _dbContext.Users.AddAsync(entity);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.NickName == name);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }
    }*/
}
