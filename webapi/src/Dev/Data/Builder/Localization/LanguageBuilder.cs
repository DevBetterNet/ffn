using Dev.Core.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Data.Builder.Users
{
    public class LanguageBuilder : DevEntityTypeConfiguration<Language>
    {
        public override void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable($"{nameof(Language)}");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.LanguageCulture).HasMaxLength(20).IsRequired();
            builder.Property(x => x.UniqueSeoCode).HasMaxLength(2).IsRequired();
            builder.Property(x => x.FlagImageFileName).HasMaxLength(50).IsRequired();
            base.Configure(builder);
        }
    }
}
