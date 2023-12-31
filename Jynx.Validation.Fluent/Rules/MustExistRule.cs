﻿using FluentValidation;
using Jynx.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Validation.Fluent.Rules
{
    public class MustExistRule<T>
    {
        private readonly IRuleBuilder<T, string?> _ruleBuilder;

        public MustExistRule(IRuleBuilder<T, string?> ruleBuilder)
        {
            _ruleBuilder = ruleBuilder;
        }

        public IRuleBuilderOptions<T, string?> Using<TRepositoryService>(IServiceProvider services)
            where TRepositoryService : IEntityService
        {
            return _ruleBuilder.MustAsync(async (v, c) =>
            {
                if (string.IsNullOrWhiteSpace(v))
                    return true;

                var service = services.GetService<TRepositoryService>();

                if (service is null)
                    return false;

                var exists = await service.ExistsAsync(v);

                return exists;
            });
        }
    }
}
