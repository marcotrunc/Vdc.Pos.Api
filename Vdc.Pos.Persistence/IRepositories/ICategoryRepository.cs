using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.IRepositories.Common;

namespace Vdc.Pos.Persistence.IRepositories
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task<IEnumerable<Category>> FindAllParentCategory();
        Task<IEnumerable<Category>> FindAllChildCategory();
        Task<bool> IsUniqueCategoryNameForThisParentCategory(string name, Guid parentId);
        Task<bool> IsUniqueParentCategoryName(string name);
    }
}
