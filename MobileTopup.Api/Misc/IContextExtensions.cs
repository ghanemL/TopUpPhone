using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MobileTopup.Infrastructure.Persistance;

namespace MobileTopup.Api.Misc
{
    public interface IContextExtensions
    {
        IQueryable<TEntity> FromSqlInterpolated<TEntity>(
            [NotNull] DbSet<TEntity> source,
            [NotNull, NotParameterized] FormattableString sql)
            where TEntity : class;

        Task<TOut> ExecuteUntestableAsync<TOut>(Func<ApplicationDbContext, Task<TOut>> func);
    }
}
