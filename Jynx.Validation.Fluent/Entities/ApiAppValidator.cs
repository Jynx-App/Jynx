using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Validation.Fluent.Entities
{
    internal class ApiAppValidator : BaseValidator<ApiApp>
    {
        public ApiAppValidator(IServiceProvider services)
            : base(services)
        {
        }

        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.UserId)
                    .NotEmpty()
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IUsersService>(Services);

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(x => x.PublicKey)
                    .NotEmpty()
                    .MaximumLength(22);

                RuleFor(x => x.PrivateKey)
                    .NotEmpty()
                    .MaximumLength(22);

                RuleFor(x => x.CallbackUrl)
                    .NotEmpty()
                    .MaximumLength(2000)
                    .Url();
            });
        }
    }
}
