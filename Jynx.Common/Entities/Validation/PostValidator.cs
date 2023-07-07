using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Common.Services;
using Jynx.Common.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common.Entities.Validation
{
    internal class PostValidator : BaseValidator<Post>
    {
        public PostValidator(IServiceProvider services)
            : base(services)
        {
        }

        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.DistrictId)
                    .NotEmpty()
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IDistrictsService>(Services);

                RuleFor(x => x.UserId)
                    .NotEmpty()
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IUsersService>(Services);

                RuleFor(x => x.EditedById)
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IUsersService>(Services);

                RuleFor(x => x.Title)
                    .NotEmpty()
                    .MaximumLength(300);

                RuleFor(x => x.Body)
                    .NotEmpty()
                        .When(x => !Uri.TryCreate(x.Url, UriKind.Absolute, out _))
                    .MaximumLength(40000);

                RuleFor(x => x.Url)
                    .NotEmpty()
                        .When(x => string.IsNullOrWhiteSpace(x.Body))
                    .Url();

                RuleFor(x => x)
                    .MustAsync(IsUserAllowedToPostAsync)
                        .WithMessage("You can not post to this district");
            });
        }

        private async Task<bool> IsUserAllowedToPostAsync(Post post, CancellationToken cancellationToken)
        {
            var districtsService = Services.GetService<IDistrictsService>();

            if (districtsService is null)
                return false;

            return await districtsService.IsUserAllowedToPostAndCommentAsync(post.DistrictId, post.UserId);
        }
    }
}
