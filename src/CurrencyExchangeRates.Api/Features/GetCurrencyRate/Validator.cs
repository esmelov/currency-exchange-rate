using FluentValidation;

namespace CurrencyExchangeRates.Api.Features.GetCurrencyRate
{
    public sealed partial class GetCurrencyRate
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Code)
                    .NotEmpty()
                    .WithMessage("Code should be present.");
            }
        }
    }
}
