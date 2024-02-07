using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileTopup.Domain.UserAggregate.Entities;

namespace MobileTopup.Infrastructure.Persistance.Configuration
{
    public class TopUpBeneficiaryConfiguration : IEntityTypeConfiguration<TopUpBeneficiary>
    {
        public void Configure(EntityTypeBuilder<TopUpBeneficiary> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedNever();
            builder.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId);
            builder.HasMany(u => u.Transactions).WithOne(u => u.TopUpBeneficiary).HasForeignKey(u => u.TopUpBeneficiaryId);
        }
    }
}
