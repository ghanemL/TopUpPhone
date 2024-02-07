using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileTopup.Domain.UserAggregate;

namespace MobileTopup.Infrastructure.Persistance.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.HasMany(u => u.Beneficiaries).WithOne(u => u.User).HasForeignKey(u => u.UserId);
        }
    }
}
