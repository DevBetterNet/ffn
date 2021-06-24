using Dev.Core.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Data.Builder.Configuration
{
    public class SettingBuilder : DevEntityTypeConfiguration<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable($"{nameof(Setting)}");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(6000).IsRequired();
            base.Configure(builder);
        }
    }
}
