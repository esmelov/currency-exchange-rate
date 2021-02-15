using System;

namespace CurrencyExchangeRates.Api.Features.GetCurrencyRate
{
    public sealed partial class GetCurrencyRate
    {
        public class Response
        {
            public Response(string currencyCode, decimal currencyRate, DateTime rateDateUtc)
            {
                CurrencyCode = currencyCode;
                CurrencyRate = currencyRate;
                RateDateUtc = rateDateUtc.ToUniversalTime();
            }

            public string CurrencyCode { get; }
            public decimal CurrencyRate { get; }
            public DateTime RateDateUtc { get; }
        }
    }
}
