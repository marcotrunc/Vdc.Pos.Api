using Slugify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Response
{
    public class BrandResponseDto : CommonResponseDto<Guid>
    {
        public string Name { get; set; } 
        public string Slug {  get; set; }
        public string ImgPath { get; set; }
        public bool IsDeleted { get; set; }
    }
}
