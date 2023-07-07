using FluentValidation;
using Jynx.Abstractions.Entities;

namespace Jynx.Common.Entities.Validation
{
    internal abstract class BaseValidator<TEntity> : AbstractValidator<TEntity>
        where TEntity : BaseEntity
    {

        protected BaseValidator(IServiceProvider services)
        {
            Services = services;

            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            ConfigureRules();
        }

        protected IServiceProvider Services { get; }

        protected static int DefaultIdMaxLength { get; } = 255;

        protected int IdMaxLength { get; set; } = DefaultIdMaxLength;

        protected virtual void ConfigureRules()
        {
            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.Id)
                    .MaximumLength(IdMaxLength);
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
