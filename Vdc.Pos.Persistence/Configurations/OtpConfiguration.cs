using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Domain.Entities.Common;
using Vdc.Pos.Persistence.Configurations.Common;

namespace Vdc.Pos.Persistence.Configurations
{
    public class OtpConfiguration : IEntityTypeConfiguration<Otp>
    {
        public void Configure(EntityTypeBuilder<Otp> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

            builder.Property(o => o.OtpCode).IsRequired();
            builder.Property(o => o.CreatedOn).HasDefaultValueSql("getutcdate()");
            builder.Property(o => o.ExpiredOn).IsRequired();

            builder.HasOne(o => o.User)
            .WithMany(o => o.Otps)
            .HasForeignKey(o => o.UserId)
            .IsRequired();
        }
    }
}
