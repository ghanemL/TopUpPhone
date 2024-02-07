using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileTopup.Domain.UserAggregate.Entities;

namespace MobileTopup.Infrastructure.Persistance.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(b => b.TransactionId);
            builder.Property(p => p.TransactionId).IsRequired().ValueGeneratedNever();
            //builder.HasOne(u => u.TopUpBeneficiary).WithMany().HasForeignKey(u => u.TopUpBeneficiaryId);
        }
    }
}
