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
    public class OtpRepository : IOtpRepository
    {
        private readonly ApplicationDataContext _dbContext;
        public OtpRepository(ApplicationDataContext dbContext)
        {
                _dbContext = dbContext ?? throw new ArgumentNullException("Db Context Non generato");
        }
        public void Delete(Otp otp)
        {
             _dbContext.Otps.Remove(otp);
        }

        public async Task<Otp?> GetByPrimaryKeyAsync(int id)
        {
            return await _dbContext.Otps.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id); 
        }

        public async ValueTask<EntityEntry<Otp>?> InsertAsync(Otp otp)
        {
            return await _dbContext.Otps.AddAsync(otp);
        }
    }
}
