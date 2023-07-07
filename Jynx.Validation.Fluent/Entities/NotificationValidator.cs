using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Validation.Fluent.Entities
{
    internal class NotificationValidator : BaseValidator<Notification>
    {
        public NotificationValidator(IServiceProvider services)
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

                RuleFor(x => x.Title)
                    .NotEmpty()
                    .MinimumLength(3)
                    .MaximumLength(100);

                RuleFor(x => x.Body)
                    .NotEmpty()
                    .MaximumLength(10000);
            });
        }
    }
}
