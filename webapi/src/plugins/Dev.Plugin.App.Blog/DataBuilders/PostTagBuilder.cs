using Dev.Data.Builder;
using Dev.Plugin.App.Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Plugin.App.Blog.DataBuilders;

public class PostTagBuilder : DevEntityTypeConfiguration<PostTag>
{
    public override void Configure(EntityTypeBuilder<PostTag> builder)
    {
        builder.ToTable($"{Constants.PluginName}{nameof(PostTag)}");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.BlogPostCount).HasDefaultValue(1);
        base.Configure(builder);
    }
}
