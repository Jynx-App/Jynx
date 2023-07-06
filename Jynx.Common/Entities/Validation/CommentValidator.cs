using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal class CommentValidator : BaseValidator<Comment>
    {
        protected override void ConfigureRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.DistrictId)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.PostId)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.ParentCommentId)
                .MaximumLength(80);

            RuleFor(x => x.Body)
                .NotEmpty()
                .MaximumLength(40000);
        }
    }
}
