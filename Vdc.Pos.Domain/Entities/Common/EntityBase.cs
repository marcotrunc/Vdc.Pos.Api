using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.Entities.Common
{
    public abstract class EntityBase<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set;}
        public DateTime? DeletedOn { get; set; }
    }
}
