using FluentValidation;

namespace Jynx.Common.Entities.Validation
{
    internal abstract class BaseValidator<TEntity> : AbstractValidator<TEntity>
    {
        protected BaseValidator()
        {
            ConfigureRules();
        }

        protected abstract void ConfigureRules();
    }
}
