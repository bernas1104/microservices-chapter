using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Users.Domain.Entities;
using Users.Domain.Interfaces.Repositories;
using Users.Infra.Context;

namespace Users.Infra.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UsersDbContext _dbContext;

        public UsersRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            _dbContext.Users.Add(user);

            await SaveChanges();

            return user;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(
                x => x.Id.ToString() == id
            );
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(
                x => x.Email == email
            );
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await SaveChanges();
        }

        private async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
