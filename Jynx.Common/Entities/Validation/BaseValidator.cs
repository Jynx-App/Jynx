using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal abstract class BaseValidator<TEntity> : AbstractValidator<TEntity>
        where TEntity : BaseEntity
    {
        protected BaseValidator()
        {
            ConfigureRules();
        }

        protected virtual void ConfigureRules()
        {
            RuleSet(ValidationMode.Default, () => {
                RuleFor(x => x.Id)
                    .MaximumLength(80);
            });

            RuleSet(ValidationMode.Update, () =>
            {
                RuleFor(x => x.Id)
                    .NotEmpty();
            });
        }

        protected virtual void RuleSet(ValidationMode validationMode, Action action)
            => RuleSet(validationMode.ToString(), action);
    }
}
