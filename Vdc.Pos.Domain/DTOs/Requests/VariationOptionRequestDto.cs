using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Requests
{
    public class VariationOptionRequestDto
    {
        public int VariationId { get; set; }
        public string Value { get; set; }
    }
}
