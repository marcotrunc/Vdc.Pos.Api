using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Response
{
    public class CategoryResponseDto : CommonResponseDto<Guid>
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; } = null;
    }
}
