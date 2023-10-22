using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Persistence.DataContext;
using static Vdc.Pos.Business.UnitOfWork.UnitOfWork;
using Vdc.Pos.Domain.Entities;

namespace Vdc.Pos.Business.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDataContext _dbcontext;

        public UnitOfWork(ApplicationDataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region public methods
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return await _dbcontext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> UpdateAsync<T>(T entity,CancellationToken cancellationToken = default) where T : class 
        {
            _dbcontext.Attach(entity);

            _dbcontext.Entry(entity).State = EntityState.Modified;
            
            return await this.CommitAsync(cancellationToken);
        }
        #endregion

        #region private methods
        private void SetTimestamps()
        {
            var entities = _dbcontext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var currentTime = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                try
                {
                    if(entity.Property("CreatedOn") == null)
                    {
                        continue;
                    }
                    if (entity.State == EntityState.Added)
                    {
                        entity.Property("CreatedOn").CurrentValue = currentTime;
                    }
                    
                    if (entity.Property("UpdatedOn") == null)
                    {
                        continue;
                    }
                    
                    entity.Property("UpdatedOn").CurrentValue = currentTime;
                }
                catch
                {
                    continue;
                }
            }
        }
        #endregion
    }
}
