using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.IRepositories.Common;

namespace Vdc.Pos.Persistence.IRepositories
{
    public interface IBrandrepository : IRepository<Brand, Guid>
    {
        Task<bool> IsUniqueBrandName(string name);
        Task<IEnumerable<Brand>> GetBrandsNotDeletedAsync();
    }
}
