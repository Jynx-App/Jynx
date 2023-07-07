using FluentValidation;
using Jynx.Abstractions.Entities;

namespace Jynx.Validation.Fluent.Entities
{
    internal class DistrictValidator : BaseValidator<District>
    {
        public DistrictValidator(IServiceProvider services)
            : base(services)
        {
            IdMaxLength = 32;
        }

        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.Id)
                    .MinimumLength(3)
                    .Matches("^[a-z][a-z0-9_-]+$");

                RuleFor(x => x.Description)
                    .NotEmpty()
                    .MaximumLength(200);
            });
        }
    }
}
