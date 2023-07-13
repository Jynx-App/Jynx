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
                RuleFor(x => x.Type)
                    .IsInEnum();

                RuleFor(x => x.UserId)
                    .NotEmpty()
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IUsersService>(Services);

                RuleFor(x => x.Title)
                    .NotEmpty()
                    .MinimumLength(3)
                    .MaximumLength(100);

                RuleFor(x => x.Body)
                    .MaximumLength(200);

                RuleFor(x => x.ForeignId)
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<ICommentsService>(Services)
                        .When(x => x.Type == NotificationType.CommentReply);
            });
        }
    }
}
