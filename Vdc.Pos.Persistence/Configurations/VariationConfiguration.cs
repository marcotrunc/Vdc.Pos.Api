using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;

namespace Vdc.Pos.Persistence.Configurations
{
    public class VariationConfiguration : IEntityTypeConfiguration<Variation>
    {
        public void Configure(EntityTypeBuilder<Variation> builder)
        {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).ValueGeneratedOnAdd();

            builder.Property(v => v.Name).IsRequired();

            builder.HasOne(v => v.Category)
            .WithMany(c => c.Variations)
            .HasForeignKey(v => v.ParentCategoryId)
            .IsRequired();

            builder.HasMany(v => v.Options)
                .WithOne(vo => vo.Variation)
                .HasForeignKey(vo => vo.VariationId)
                .IsRequired();
        }
    }
}
