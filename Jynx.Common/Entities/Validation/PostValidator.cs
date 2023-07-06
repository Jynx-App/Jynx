using FluentValidation;
using Jynx.Common.Validation;

namespace Jynx.Common.Entities.Validation
{
    internal class PostValidator : BaseValidator<Post>
    {
        protected override void ConfigureRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.DistrictId)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.UserId)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.EditedById)
                .MaximumLength(80);

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
        }
    }
}
