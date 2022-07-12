using Dev.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Data.Builder;

public abstract class DevEntityTypeConfiguration<TEntity> : IBuilderConfiguration, IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    #region Methods

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
    }

    public virtual void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(this);
    }

    #endregion Methods
}