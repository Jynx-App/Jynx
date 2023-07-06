using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal class DistrictUserValidator : BaseValidator<DistrictUser>
    {
        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.DistrictId)
                .NotEmpty()
                .MaximumLength(45);

                RuleFor(x => x.DistrictUserGroupId)
                    .MaximumLength(80);

                RuleForEach(x => x.ModerationPermissions)
                    .IsInEnum();

                RuleFor(x => x.BanReason)
                    .MaximumLength(200);
            });
        }
    }
}
