using FluentValidation;

namespace Jynx.Validation.Fluent
{
    internal static class IRuleBuilderExtensions
    {
        public static IRuleBuilder<T, string?> Url<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            ruleBuilder.Must(x =>
            {
                if (string.IsNullOrWhiteSpace(x))
                    return true;

                return Uri.TryCreate(x, UriKind.Absolute, out _);
            });

            return ruleBuilder;
        }
    }
}
