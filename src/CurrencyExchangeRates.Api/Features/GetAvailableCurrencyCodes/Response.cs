using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyExchangeRates.Api.Features.GetAvailableCurrencyCodes
{
    public sealed partial class GetAvailableCurrencyCodes
    {
        public class Response<T>
        {
            public Response(int @from, long totalCount, IEnumerable<T> items, DateTime forDate)
            {
                Items = items?.ToArray() ?? Array.Empty<T>();
                From = @from;
                TotalCount = totalCount;
                ForDateUtc = forDate.ToUniversalTime().Date;
            }

            public int From { get; }
            public int Count => Items.Count;
            public long TotalCount { get; }
            public DateTime ForDateUtc { get; }
            public IReadOnlyList<T> Items { get; }

            public static Response<T> Empty()
                => new (1, 0, null, DateTime.UtcNow);
        }
    }
}
