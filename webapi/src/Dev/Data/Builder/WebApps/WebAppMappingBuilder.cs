using Dev.Core.Domain.WebApps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Data.Builder.WebApps
{
    public class WebAppMappingBuilder : DevEntityTypeConfiguration<WebAppMapping>
    {
        public override void Configure(EntityTypeBuilder<WebAppMapping> builder)
        {
            builder.ToTable($"{nameof(WebAppMapping)}");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.EntityName).HasMaxLength(400).IsRequired();
            builder.Property(x => x.WebAppId).IsRequired();
            base.Configure(builder);
        }
    }
}
