using Cbr.Client.Contracts;
using MediatR;

namespace CurrencyExchangeRates.Api.Features.GetAvailableCurrencyCodes
{
    public sealed partial class GetAvailableCurrencyCodes
    {
        public class Request : IRequest<Response<CurrencyInfo>>
        {
            public Request()
            { }

            public Request(int skip, int take)
            {
                Skip = skip;
                Take = take;
            }

            /// <summary>
            /// Number of currencies which should be skip
            /// </summary>
            public int Skip { get; set; } = 0;

            /// <summary>
            /// Number of currencies which should be return
            /// </summary>
            public int Take { get; set; } = 25;
        }
    }
}
