using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.DataContext;
using Vdc.Pos.Persistence.IRepositories;

namespace Vdc.Pos.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDataContext _dbContext;
        public UserRepository(ApplicationDataContext dbContext) 
        { 
            _dbContext = dbContext ?? throw new ArgumentNullException("Db Context Non generato");
        }

        public async Task DeleteByPrimaryKeyAsync(Guid id)
        {
            var userToDelete = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (userToDelete != null)
            {
                _dbContext.Users.Remove(userToDelete);
            }
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> FindByFullNameAsync(string name, string fullName)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name && x.LastName == fullName);
        }

        public async Task<User?> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByPrimaryKeyAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<Guid> GetUserIdFromEmailAsync(string email)
        {
            return await _dbContext.Users.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).FirstAsync();
        }

        public async ValueTask<EntityEntry<User>?> InsertAsync(User entity)
        {
             return await _dbContext.Users.AddAsync(entity);
        }

        public async Task<bool> IsUniqueEmail(string email)
        {
            return !await _dbContext.Users.AnyAsync(x => x.Email == email);
        }
    }
}
