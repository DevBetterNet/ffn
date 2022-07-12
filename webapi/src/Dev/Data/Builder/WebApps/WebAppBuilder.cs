using Dev.Core.Domain.WebApps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Data.Builder.WebApps;

public class WebAppBuilder : DevEntityTypeConfiguration<WebApp>
{
    public override void Configure(EntityTypeBuilder<WebApp> builder)
    {
        builder.ToTable($"{nameof(WebApp)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(400).IsRequired();
        builder.Property(x => x.Url).HasMaxLength(400).IsRequired();
        builder.Property(x => x.Hosts).HasMaxLength(1000).IsRequired();
        base.Configure(builder);
    }
}
