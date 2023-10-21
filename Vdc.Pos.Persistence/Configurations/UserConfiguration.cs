using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.Configurations.Common;

namespace Vdc.Pos.Persistence.Configurations
{
    public class UserConfiguration : EntityBaseConfiguration<User, Guid>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            
            builder.Property(x => x.Email).IsUnicode(true).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(80);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(120);
            builder.Property(x => x.PhoneNumber).IsUnicode().HasMaxLength(20);

            builder.HasMany(e => e.Otps)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .IsRequired();
        }
    }
}
