using MobileTopup.Infrastructure.Persistance;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MobileTopup.Api.Misc
{
    public class ContextExtensions : IContextExtensions
    {
        private readonly ApplicationDbContext _context;

        public ContextExtensions(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> FromSqlRaw<TEntity>(
           [NotNull] DbSet<TEntity> source,
           [NotNull][NotParameterized] string sql,
           [NotNull] params object[] parameters)
           where TEntity : class
        {
            return source.FromSqlRaw(sql, parameters);
        }

        public IQueryable<TEntity> FromSqlInterpolated<TEntity>(
            [NotNull] DbSet<TEntity> source,
            [NotNull, NotParameterized] FormattableString sql) where TEntity : class
        {
            return source.FromSqlInterpolated(sql);
        }

        public Task<TOut> ExecuteUntestableAsync<TOut>(Func<ApplicationDbContext, Task<TOut>> func)
        {
            return func(_context);
        }
    }
}
