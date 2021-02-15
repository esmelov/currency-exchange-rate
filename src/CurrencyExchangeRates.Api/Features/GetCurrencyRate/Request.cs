using MediatR;

namespace CurrencyExchangeRates.Api.Features.GetCurrencyRate
{
    public sealed partial class GetCurrencyRate
    {
        public class Request : IRequest<Response>
        {
            public Request()
            { }

            public Request(string code)
            {
                Code = code;
            }

            /// <summary>
            /// Requested currency code.
            /// </summary>
            public string Code { get; set; }
        }
    }
}
