using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MobileTopup.Application.Common.Interfaces.Persistance;
using MobileTopup.Infrastructure.Persistance;
using MobileTopup.Infrastructure.Persistance.Repositories;

namespace MobileTopup.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {

            return services;
        }

        public static IServiceCollection AddPersistence(
            this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.ConfigureWarnings(c => c.Ignore(CoreEventId.MultipleNavigationProperties));
                    options.UseSqlServer(
                        @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TopUpDB;Integrated Security=False;Connect Timeout=30;MultipleActiveResultSets=true",
                        builder =>
                        {
                            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                        });
                },
                ServiceLifetime.Transient);

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
