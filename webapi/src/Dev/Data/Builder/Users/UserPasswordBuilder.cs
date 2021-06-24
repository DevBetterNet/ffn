using Dev.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dev.Data.Builder.Users
{
    public class UserPasswordBuilder : DevEntityTypeConfiguration<UserPassword>
    {
        public override void Configure(EntityTypeBuilder<UserPassword> builder)
        {
            builder.ToTable($"{nameof(UserPassword)}");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Password).HasMaxLength(100).IsRequired();
            builder.Property(x => x.PasswordSalt).HasMaxLength(6);
            builder.Ignore(x => x.PasswordFormat);
            base.Configure(builder);
        }
    }
}
