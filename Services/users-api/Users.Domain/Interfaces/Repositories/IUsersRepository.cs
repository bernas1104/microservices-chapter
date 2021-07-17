using System.Collections.Generic;
using System.Threading.Tasks;
using Users.Domain.Entities;

namespace Users.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task UpdateAsync(User user);
    }
}
