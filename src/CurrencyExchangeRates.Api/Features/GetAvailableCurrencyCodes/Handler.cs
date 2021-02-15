using Cbr.Client.Contracts;
using CurrencyExchangeRates.Api.DAL;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyExchangeRates.Api.Features.GetAvailableCurrencyCodes
{
    public sealed partial class GetAvailableCurrencyCodes
    {
        public class Handler : IRequestHandler<Request, Response<CurrencyInfo>>
        {
            private readonly IRepository<CurrencyInfo> _repository;

            public Handler(IRepository<CurrencyInfo> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<Response<CurrencyInfo>> Handle(Request request, CancellationToken cancellationToken)
            {
                var totalCount = await _repository.Count(cancellationToken);
                if (totalCount == 0)
                    return Response<CurrencyInfo>.Empty();

                var items = await _repository.Find(request.Skip, request.Take, cancellationToken);

                return new Response<CurrencyInfo>(request.Skip + 1, totalCount, items, DateTime.UtcNow);
            }
        }
    }
}
