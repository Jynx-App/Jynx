using FluentValidation;
using Jynx.Abstractions.Entities;

namespace Jynx.Common.Entities.Validation
{
    internal class UserValidator : BaseValidator<User>
    {
        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.Username)
                .NotEmpty()
                .Matches("^[a-zA-Z][a-zA-Z0-9_-]+$")
                .MaximumLength(32);

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(320);

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MaximumLength(1024);

                RuleFor(x => x.BanReason)
                    .MaximumLength(200);
            });
        }
    }
}
