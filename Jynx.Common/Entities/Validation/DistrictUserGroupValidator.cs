using FluentValidation;
using Jynx.Abstractions.Entities;

namespace Jynx.Common.Entities.Validation
{
    internal class DistrictUserGroupValidator : BaseValidator<DistrictUserGroup>
    {
        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.DistrictId)
                .NotEmpty()
                .MaximumLength(80);

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MinimumLength(3)
                    .MaximumLength(32);

                RuleFor(x => x.Description)
                    .MaximumLength(200);

                RuleForEach(x => x.ModerationPermissions)
                    .IsInEnum();
            });
        }
    }
}
