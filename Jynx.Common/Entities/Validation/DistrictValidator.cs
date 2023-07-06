using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal class DistrictValidator : BaseValidator<District>
    {
        protected override void ConfigureRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Matches("^[a-z][a-z0-9_-]+$")
                .MinimumLength(3)
                .MaximumLength(32);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
