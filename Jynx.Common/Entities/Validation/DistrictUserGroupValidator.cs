using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Common.Entities.Validation
{
    internal class DistrictUserGroupValidator : BaseValidator<DistrictUserGroup>
    {
        public DistrictUserGroupValidator(IServiceProvider services)
            : base(services)
        {
        }

        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.DistrictId)
                    .NotEmpty()
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IDistrictsService>(Services);

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
