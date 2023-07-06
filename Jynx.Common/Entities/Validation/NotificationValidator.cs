﻿using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal class NotificationValidator : BaseValidator<Notification>
    {
        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.UserId)
                .NotEmpty()
                .MaximumLength(80);

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