using Dev.Data.Builder;
using Dev.Plugin.App.Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Plugin.App.Blog.DataBuilders
{
    public class PostBuilder : DevEntityTypeConfiguration<Post>
    {
        public override void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable($"{Constants.PluginName}{nameof(Post)}");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.MetaKeywords).HasMaxLength(400);
            builder.Property(p => p.MetaTitle).HasMaxLength(400);
            builder.Property(p => p.MetaDescription).HasMaxLength(4000);
            base.Configure(builder);
        }
    }
}
