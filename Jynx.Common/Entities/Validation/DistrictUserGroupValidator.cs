using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal class DistrictUserGroupValidator : BaseValidator<DistrictUserGroup>
    {
        protected override void ConfigureRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MaximumLength(80);

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
        }
    }
}
