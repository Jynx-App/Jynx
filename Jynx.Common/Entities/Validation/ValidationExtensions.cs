using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Common.Validation.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common.Entities.Validation
{
    internal static class ValidationExtensions
    {
        public static IServiceCollection AddEntityValidators(this IServiceCollection services)
        {
            services
                .AddScoped<IValidator<ApiApp>, ApiAppValidator>()
                .AddScoped<IValidator<ApiAppUser>, ApiAppUserValidator>()
                .AddScoped<IValidator<Comment>, CommentValidator>()
                .AddScoped<IValidator<DistrictUserGroup>, DistrictUserGroupValidator>()
                .AddScoped<IValidator<DistrictUser>, DistrictUserValidator>()
                .AddScoped<IValidator<District>, DistrictValidator>()
                .AddScoped<IValidator<Notification>, NotificationValidator>()
                .AddScoped<IValidator<Post>, PostValidator>()
                .AddScoped<IValidator<PostVote>, PostVoteValidator>()
                .AddScoped<IValidator<User>, UserValidator>();

            return services;
        }

        public static MustExistRule<T> MustExist<T>(this IRuleBuilder<T, string?> ruleBuilder)
            => new(ruleBuilder);
    }
}
