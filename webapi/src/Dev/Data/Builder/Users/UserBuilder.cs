using Dev.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Data.Builder.Users
{
    public class UserBuilder : DevEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"{nameof(User)}");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FailedLoginAttempts).HasDefaultValue(0);
            builder.Property(x => x.Active).HasDefaultValue(true);
            builder.Property(x => x.Deleted).HasDefaultValue(false);
            builder.Property(x => x.LastIpAddress).HasMaxLength(100);
            base.Configure(builder);
        }
    }
}
