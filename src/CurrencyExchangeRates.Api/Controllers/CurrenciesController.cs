using Cbr.Client.Contracts;
using CurrencyExchangeRates.Api.Features.GetAvailableCurrencyCodes;
using CurrencyExchangeRates.Api.Features.GetCurrencyRate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyExchangeRates.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrenciesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("codes")]
        [ProducesResponseType(typeof(GetAvailableCurrencyCodes.Response<CurrencyInfo>), StatusCodes.Status200OK)]
        public Task<GetAvailableCurrencyCodes.Response<CurrencyInfo>> GetAvailableCurrencyCodesAsync(
            [FromQuery] GetAvailableCurrencyCodes.Request request, CancellationToken token)
        {
            return _mediator.Send(request, token);
        }

        [HttpGet("{code}/rate")]
        [ProducesResponseType(typeof(GetCurrencyRate.Response), StatusCodes.Status200OK)]
        public Task<GetCurrencyRate.Response> GetCurrencyRateAsync(
            [FromRoute] GetCurrencyRate.Request request, CancellationToken token)
        {
            return _mediator.Send(request, token);
        }
    }
}
