using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Validation.Fluent.Entities
{
    internal class UserValidator : BaseValidator<User>
    {
        public UserValidator(IServiceProvider services)
            : base(services)
        {
        }

        protected override void ConfigureRules()
        {
            base.ConfigureRules();

            RuleSet(ValidationMode.Default, () =>
            {
                RuleFor(x => x.Username)
                    .NotEmpty()
                    .MaximumLength(32)
                    .Matches("^[a-zA-Z][a-zA-Z0-9_-]+$")
                    .MustAsync(IsUsernameUniqueAsync)
                        .WithMessage("Username is not unique");

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .MaximumLength(320)
                    .EmailAddress();

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MaximumLength(1024);

                RuleFor(x => x.BanReason)
                    .MaximumLength(200);
            });
        }

        private async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            var usersService = Services.GetService<IUsersService>();

            if (usersService is null)
                return false;

            return await usersService.IsUsernameUnique(username);
        }
    }
}
