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
    public class VariationOptionConfiguration : IEntityTypeConfiguration<VariationOption>
    {
        public void Configure(EntityTypeBuilder<VariationOption> builder)
        {
            builder.HasKey(vo => vo.Id);
            builder.Property(vo => vo.Id).ValueGeneratedOnAdd();

            builder.Property(vo => vo.Value).IsRequired();

            builder.HasOne(vo => vo.Variation)
            .WithMany(v => v.Options)
            .HasForeignKey(vo => vo.VariationId)
            .IsRequired();
        }
    }
}
