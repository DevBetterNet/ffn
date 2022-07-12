using Dev.Data.Builder;
using Dev.Plugin.App.Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Plugin.App.Blog.DataBuilders;

public class CategoryBuilder : DevEntityTypeConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable($"{Constants.PluginName}{nameof(Category)}");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.MetaKeywords).HasMaxLength(400);
        builder.Property(p => p.MetaTitle).HasMaxLength(400);
        builder.Property(p => p.MetaDescription).HasMaxLength(4000);
        base.Configure(builder);
    }
}
