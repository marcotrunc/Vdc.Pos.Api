using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.Entities
{
    public class Variation
    {
        public int Id { get; set; }
        public Guid ParentCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Category Category { get; set; }
        public ICollection<VariationOption> Options { get; set; }
    }
}
