using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Validation.Fluent.Entities
{
    internal class PostVoteValidator : BaseValidator<PostVote>
    {
        public PostVoteValidator(IServiceProvider services)
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

                RuleFor(x => x.PostId)
                    .NotEmpty()
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IPostsService>(Services);
            });
        }
    }
}
