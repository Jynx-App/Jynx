using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Validation.Fluent.Entities
{
    internal class CommentValidator : BaseValidator<Comment>
    {
        public CommentValidator(IServiceProvider services)
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

                RuleFor(x => x.PostId)
                    .NotEmpty()
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<IPostsService>(Services)
                    .MustAsync(PostIsUnlocked);

                RuleFor(x => x.ParentCommentId)
                    .MaximumLength(DefaultIdMaxLength)
                    .MustExist().Using<ICommentsService>(Services);

                RuleFor(x => x.Body)
                    .NotEmpty()
                    .MaximumLength(40000);

                RuleFor(x => x)
                    .MustAsync(IsUserAllowedToCommentAsync)
                        .WithMessage("You can not comment to this district");
            });
        }

        private async Task<bool> IsUserAllowedToCommentAsync(Comment comment, CancellationToken cancellationToken)
        {
            var districtsService = Services.GetService<IDistrictsService>();

            if (districtsService is null)
                return false;

            return await districtsService.IsUserAllowedToPostAndCommentAsync(comment.DistrictId, comment.UserId);
        }

        private async Task<bool> PostIsUnlocked(string? postId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(postId))
                return true;

            var postsService = Services.GetService<IPostsService>();

            if (postsService is null)
                return true;

            var post = await postsService.GetAsync(postId);

            if (post is null)
                return true;

            return !post.CommentsLocked;
        }
    }
}
