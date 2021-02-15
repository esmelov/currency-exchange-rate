using Cbr.Client.Abstractions;
using Cbr.Client.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyExchangeRates.Api.DAL
{
    internal sealed class CurrencyRepository : IRepository<CurrencyInfo>
    {
        private readonly ICbrClient _client;
        private readonly IMemoryCache _cache;

        public CurrencyRepository(ICbrClient client, IMemoryCache cache)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public IQueryable<CurrencyInfo> Query => GetCurrencyInfoQuery(CancellationToken.None)
            .ConfigureAwait(false)
            .GetAwaiter().GetResult();

        public async Task<CurrencyInfo> FindOneOrDefault(CancellationToken token)
        {
            var query = await GetCurrencyInfoQuery(token);

            return query.FirstOrDefault();
        }

        public async Task<IReadOnlyList<CurrencyInfo>> Find(int skip, int take, CancellationToken token)
        {
            var query = await GetCurrencyInfoQuery(token);

            return query.OrderBy(x => x.Id).Skip(skip).Take(take).ToArray();
        }

        public async Task<CurrencyInfo> FindOneOrDefault(Expression<Func<CurrencyInfo, bool>> condition, CancellationToken token)
        {
            CheckCondition(condition);

            var query = await GetCurrencyInfoQuery(token);

            return query.FirstOrDefault(condition);
        }

        public async Task<IReadOnlyList<CurrencyInfo>> Find(Expression<Func<CurrencyInfo, bool>> condition,
            int skip, int take, CancellationToken token)
        {
            CheckCondition(condition);

            var query = await GetCurrencyInfoQuery(token);

            return query.Where(condition).OrderBy(x => x.Id).Skip(skip).Take(take).ToArray();
        }

        public async Task<long> Count(CancellationToken token)
        {
            var query = await GetCurrencyInfoQuery(token);

            return query.Count();
        }

        public async Task<long> Count(Expression<Func<CurrencyInfo, bool>> condition, CancellationToken token)
        {
            CheckCondition(condition);

            var query = await GetCurrencyInfoQuery(token);

            return query.Count(condition);
        }

        private async Task<IQueryable<CurrencyInfo>> GetCurrencyInfoQuery(CancellationToken token)
        {
            var rates = GetCachedDailyValue();
            if (rates == null)
            {
                rates = await _client.GetCurrentDayRates(token);
                if (rates == null)
                    return Array.Empty<CurrencyInfo>().AsQueryable();

                AddCachedDailyValue(rates);
            }

            return rates.Valute?.Values.AsQueryable() ?? Array.Empty<CurrencyInfo>().AsQueryable();
        }

        private DailyRates GetCachedDailyValue()
        {
            var cacheKey = $"daily:{DateTime.UtcNow.Date.ToShortDateString()}";
            _cache.TryGetValue(cacheKey, out DailyRates rates);
            return rates;
        }

        private void AddCachedDailyValue(DailyRates item)
        {
            var nowDate = DateTime.UtcNow.Date;
            var cacheKey = $"daily:{nowDate.ToShortDateString()}";
            _cache.Set(cacheKey, item,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(nowDate.AddHours(6)));
        }

        private static void CheckCondition<T>(Expression<Func<T, bool>> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
        }
    }
}
