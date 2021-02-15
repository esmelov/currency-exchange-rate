using FluentValidation;

namespace CurrencyExchangeRates.Api.Features.GetAvailableCurrencyCodes
{
    public sealed partial class GetAvailableCurrencyCodes
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Skip)
                    .Must(x => x >= 0)
                    .WithMessage("Skip must be positive number.");
                RuleFor(x => x.Take)
                    .Must(x => x > 0 && x <= 25)
                    .WithMessage("Take should be in range between 1 and 25.");
            }
        }
    }
}
