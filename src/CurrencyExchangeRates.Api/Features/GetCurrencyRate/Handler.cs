using Cbr.Client.Abstractions;
using Cbr.Client.Contracts;
using CurrencyExchangeRates.Api.DAL;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyExchangeRates.Api.Features.GetCurrencyRate
{
    public sealed partial class GetCurrencyRate
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IRepository<CurrencyInfo> _repository;
            private readonly ICbrClient _client;

            public Handler(IRepository<CurrencyInfo> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var currencyInfo = await _repository.FindOneOrDefault(x =>
                    x.CharCode.Equals(request.Code, StringComparison.OrdinalIgnoreCase),
                    cancellationToken);

                return new Response(request.Code, currencyInfo.Value, DateTime.UtcNow.Date);
            }
        }
    }
}
