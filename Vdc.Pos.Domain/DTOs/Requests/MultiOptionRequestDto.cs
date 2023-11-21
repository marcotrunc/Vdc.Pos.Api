using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Requests
{
    public class MultiOptionRequestDto
    {
        public int VariationId { get; set; }
        public string[] Values { get; set; }
    }
}
