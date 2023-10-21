using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Domain.Entities.Common;

namespace Vdc.Pos.Persistence.IRepositories.Common
{
    public interface IRepository<TEntity, TPrimaryKey>
        where TEntity : EntityBase<TPrimaryKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByPrimaryKeyAsync(TPrimaryKey id);
        ValueTask<EntityEntry<TEntity>?> InsertAsync(TEntity entity);
        Task DeleteByPrimaryKeyAsync(TPrimaryKey id);

    }
}
