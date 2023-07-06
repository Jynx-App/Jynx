using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal class DistrictUserValidator : BaseValidator<DistrictUser>
    {
        protected override void ConfigureRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.DistrictId)
                .NotEmpty()
                .MaximumLength(45);

            RuleFor(x => x.DistrictUserGroupId)
                .NotEmpty()
                .MaximumLength(80);

            RuleForEach(x => x.ModerationPermissions)
                .IsInEnum();

            RuleFor(x => x.BanReason)
                .MaximumLength(200);
        }
    }
}
