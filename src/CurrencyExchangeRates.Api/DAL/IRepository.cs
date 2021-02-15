using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyExchangeRates.Api.DAL
{
    public interface IRepository<T>
    {
        IQueryable<T> Query { get; }

        Task<T> FindOneOrDefault(CancellationToken token);
        Task<T> FindOneOrDefault(Expression<Func<T, bool>> condition, CancellationToken token);

        Task<IReadOnlyList<T>> Find(int skip, int take, CancellationToken token);
        Task<IReadOnlyList<T>> Find(Expression<Func<T, bool>> condition, 
            int skip, int take, CancellationToken token);

        Task<long> Count(CancellationToken token);
        Task<long> Count(Expression<Func<T, bool>> condition, CancellationToken token);
    }
}
