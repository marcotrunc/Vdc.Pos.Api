using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Response
{
    public class CategoryResponseDto
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public Guid? ParentId { get; set; } = null;
        public DateTime CreatedOn { get; set; } 
        public DateTime UpdatedOn { get; set;}
        public DateTime? DeletedOn { get; set; }
    }
}
