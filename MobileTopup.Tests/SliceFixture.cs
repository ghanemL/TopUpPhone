using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobileTopup.Api;
using MobileTopup.Domain;
using MobileTopup.Infrastructure.Persistance;
using Respawn;
using Xunit;

namespace MobileTopup.IntegrationTests;

[CollectionDefinition(nameof(SliceFixture))]
public class SliceFixtureCollection : ICollectionFixture<SliceFixture> { }

public class SliceFixture : IAsyncLifetime
{
    private Respawner _respawner;
    //private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TopUpDB;Integrated Security=False;Connect Timeout=30;MultipleActiveResultSets=true";

    public SliceFixture()
    {
        _factory = new MobileTopUpTestApplicationFactory();

        //_configuration = _factory.Services.GetRequiredService<IConfiguration>();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

    }

    class MobileTopUpTestApplicationFactory
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ConnectionStrings:DefaultConnection", _connectionString}
                });
            });
        }

        private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TopUpDB;Integrated Security=False;Connect Timeout=30;MultipleActiveResultSets=true";
    }

    public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {

            await action(scope.ServiceProvider);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            var result = await action(scope.ServiceProvider);

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task ExecuteDbContextAsync(Func<ApplicationDbContext, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()));

    public Task ExecuteDbContextAsync(Func<ApplicationDbContext, ValueTask> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()).AsTask());

    public Task ExecuteDbContextAsync(Func<ApplicationDbContext, IMediator, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>(), sp.GetService<IMediator>()));

    public Task<T> ExecuteDbContextAsync<T>(Func<ApplicationDbContext, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()));

    public Task<T> ExecuteDbContextAsync<T>(Func<ApplicationDbContext, ValueTask<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()).AsTask());

    public Task<T> ExecuteDbContextAsync<T>(Func<ApplicationDbContext, IMediator, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>(), sp.GetService<IMediator>()));

    public Task InsertAsync<T>(params T[] entities) where T : class
    {
        return ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }
            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
        where TEntity : class
        where TEntity2 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3)
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);
            db.Set<TEntity3>().Add(entity3);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2, TEntity3 entity3, TEntity4 entity4)
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class
        where TEntity4 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);
            db.Set<TEntity3>().Add(entity3);
            db.Set<TEntity4>().Add(entity4);

            return db.SaveChangesAsync();
        });
    }

    public Task<T> FindAsync<T>(Guid id)
        where T : class, IEntity
    {
        return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(id).AsTask());
    }

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Send(request);
        });
    }

    public Task SendAsync(IRequest request)
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Send(request);
        });
    }

    private int _courseNumber = 1;

    public int NextCourseNumber() => Interlocked.Increment(ref _courseNumber);

    public async Task InitializeAsync()
    {
        _respawner = await Respawner.CreateAsync(_connectionString);

        await _respawner.ResetAsync(_connectionString);
    }

    public Task DisposeAsync()
    {
        _factory?.Dispose();
        return Task.CompletedTask;
    }
}

