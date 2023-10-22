using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.IRepositories.Common;

namespace Vdc.Pos.Persistence.IRepositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByFullNameAsync(string name, string fullName);
        Task<User?> FindByPhoneNumberAsync(string phoneNumber);
        Task<Guid> GetUserIdFromEmailAsync(string email);
        Task<bool> IsUniqueEmail (string email);
    }
}
