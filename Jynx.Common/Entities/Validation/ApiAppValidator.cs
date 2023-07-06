using FluentValidation;
using Jynx.Common.Validation;

namespace Jynx.Common.Entities.Validation
{
    internal class ApiAppValidator : BaseValidator<ApiApp>
    {
        protected override void ConfigureRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.UserId)
                .NotEmpty()
                .MaximumLength(22);

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
                .Url()
                .MaximumLength(2000);
        }
    }
}
