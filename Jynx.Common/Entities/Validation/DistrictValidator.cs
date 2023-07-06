using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal class DistrictValidator : BaseValidator<District>
    {
        protected override void ConfigureRules()
        {
            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.Id)
                .Matches("^[a-z][a-z0-9_-]+$")
                .MinimumLength(3)
                .MaximumLength(32);

                RuleFor(x => x.Description)
                    .NotEmpty()
                    .MaximumLength(200);
            });

            RuleSet(ValidationMode.Update.ToString(), () =>
            {
                RuleFor(x => x.Id)
                    .NotEmpty();
            });
        }
    }
}
