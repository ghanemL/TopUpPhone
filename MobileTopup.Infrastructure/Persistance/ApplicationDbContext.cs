
using Microsoft.EntityFrameworkCore;
using MobileTopup.Domain.UserAggregate;
using MobileTopup.Domain.UserAggregate.Entities;

namespace MobileTopup.Infrastructure.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TopUpBeneficiary> TopUpBeneficiaries { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
