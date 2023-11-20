using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.Configurations.Common;

namespace Vdc.Pos.Persistence.Configurations
{
    public class CategoryConfiguration : EntityBaseConfiguration<Category, Guid>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);

            builder.HasOne(c => c.Parent)
                .WithMany()
                .HasForeignKey(c => c.ParentId)
                .IsRequired(false);
            
            builder.HasMany(c => c.Variations)
                .WithOne(v => v.Category)
                .HasForeignKey(v => v.ParentCategoryId)
                .IsRequired();
        }
    }
}
