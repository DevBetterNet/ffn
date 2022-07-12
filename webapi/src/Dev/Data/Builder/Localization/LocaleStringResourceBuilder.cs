using Dev.Core.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Data.Builder.Users;

public class LocaleStringResourceBuilder : DevEntityTypeConfiguration<LocaleStringResource>
{
    public override void Configure(EntityTypeBuilder<LocaleStringResource> builder)
    {
        builder.ToTable($"{nameof(LocaleStringResource)}");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ResourceName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ResourceValue).HasMaxLength(int.MaxValue).IsRequired();
        builder.Property(x => x.LanguageId).IsRequired();
        base.Configure(builder);
    }
}
