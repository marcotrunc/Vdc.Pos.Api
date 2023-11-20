using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.Entities
{
    public class VariationOption
    {
        public int Id { get; set; }
        public int VariationId { get; set; }
        public string Value { get; set; }
        public Variation Variation { get; set; }
    }
}
