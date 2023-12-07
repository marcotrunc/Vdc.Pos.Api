using Slugify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vdc.Pos.Domain.Entities.Common;

namespace Vdc.Pos.Domain.Entities
{
    public class Brand : EntityBase<Guid>
    {
        public string Name { get; set; } = string.Empty;
        private string slug;
        public string Slug
        {
            get { return slug; }
            set
            {
                SlugHelper helper = new SlugHelper();
                slug = helper.GenerateSlug(value);
            }
        }
        public string? ImgPath { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
