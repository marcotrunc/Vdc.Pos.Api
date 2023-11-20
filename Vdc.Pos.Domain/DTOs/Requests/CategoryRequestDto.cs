using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Requests
{
    public class CategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; } = null;
    }
}
