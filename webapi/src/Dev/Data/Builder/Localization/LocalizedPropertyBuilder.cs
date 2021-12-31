using Dev.Core.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Data.Builder.Users
{
    public class LocalizedPropertyBuilder : DevEntityTypeConfiguration<LocalizedProperty>
    {
        public override void Configure(EntityTypeBuilder<LocalizedProperty> builder)
        {
            builder.ToTable($"{nameof(LocalizedProperty)}");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.LocaleKeyGroup).HasMaxLength(400).IsRequired();
            builder.Property(x => x.LocaleKey).HasMaxLength(400).IsRequired();
            builder.Property(x => x.LocaleValue).HasMaxLength(int.MaxValue).IsRequired();
            builder.Property(x => x.LanguageId).IsRequired();
            base.Configure(builder);
        }
    }
}
