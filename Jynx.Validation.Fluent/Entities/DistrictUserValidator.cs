using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Validation.Fluent.Entities
{
    internal class DistrictUserValidator : BaseValidator<DistrictUser>
    {
        public DistrictUserValidator(IServiceProvider services)
            : base(services)
        {
        }

        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.Id)
                    .MustExist().Using<IUsersService>(Services);

                RuleFor(x => x.DistrictId)
                    .NotEmpty()
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IDistrictsService>(Services);

                RuleFor(x => x.DistrictUserGroupId)
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IDistrictUserGroupsService>(Services);

                RuleForEach(x => x.ModerationPermissions)
                    .IsInEnum();

                RuleFor(x => x.BanReason)
                    .MaximumLength(200);
            });
        }
    }
}
