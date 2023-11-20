using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Response
{
    public class VariationOptionResponse
    {
        public int Id { get; set; }
        public int VariationId { get; set; }
        public string Value { get; set; }
    }
}
