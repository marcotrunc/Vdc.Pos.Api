using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vdc.Pos.Domain.Entities.Common;

namespace Vdc.Pos.Persistence.Configurations.Common
{
    public abstract class EntityBaseConfiguration<T, TPrimaryKey> : IEntityTypeConfiguration<T>
    where T:EntityBase<TPrimaryKey>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
