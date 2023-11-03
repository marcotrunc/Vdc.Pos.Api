using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities.Common;

namespace Vdc.Pos.Domain.Entities
{
    public class Category : EntityBase<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; } = null;
        public Category? Parent { get; set; } = null;

    }
}
