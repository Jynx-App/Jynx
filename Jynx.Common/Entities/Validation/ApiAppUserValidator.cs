using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal class ApiAppUserValidator : BaseValidator<ApiAppUser>
    {
        protected override void ConfigureRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.ApiAppId)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.UserId)
                .NotEmpty()
                .MaximumLength(80);
        }
    }
}
