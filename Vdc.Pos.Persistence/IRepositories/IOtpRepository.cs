using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.IRepositories.Common;

namespace Vdc.Pos.Persistence.IRepositories
{
    public interface IOtpRepository 
    {
        Task<Otp?> GetByPrimaryKeyAsync(int id);
        ValueTask<EntityEntry<Otp>?> InsertAsync(Otp otp);
        void Delete(Otp otp);
    }
}
