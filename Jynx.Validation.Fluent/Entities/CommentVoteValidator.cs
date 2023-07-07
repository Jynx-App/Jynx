using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Validation.Fluent.Entities
{
    internal class CommentVoteValidator : BaseValidator<CommentVote>
    {
        public CommentVoteValidator(IServiceProvider services)
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

                RuleFor(x => x.CommentId)
                    .NotEmpty()
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<ICommentsService>(Services);
            });
        }
    }
}
