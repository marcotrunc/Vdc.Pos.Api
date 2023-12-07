using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.Configurations.Common;

namespace Vdc.Pos.Persistence.Configurations
{
    public class BrandConfiguration : EntityBaseConfiguration<Brand, Guid>
    {
        public override void Configure(EntityTypeBuilder<Brand> builder)
        {
            base.Configure(builder);

            builder.Property(b => b.Name).IsUnicode().IsRequired().HasMaxLength(80);
            builder.Property(b => b.Slug).IsUnicode().IsRequired().HasMaxLength(120);
            builder.Property(b => b.ImgPath).IsRequired(false);
            builder.Property(b => b.IsDeleted).HasDefaultValue(false);
        }
    }
}
