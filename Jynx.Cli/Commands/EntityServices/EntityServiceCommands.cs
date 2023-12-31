﻿using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Services.Exceptions;
using Newtonsoft.Json;

namespace Jynx.Cli.Commands.RepositoryServices
{
    internal abstract class EntityServiceCommands<TService, TEntity> : ConsoleAppBase
        where TService : IEntityService<TEntity>
        where TEntity : BaseEntity
    {
        public EntityServiceCommands(TService service)
        {
            Service = service;
        }

        public TService Service { get; }

        [Description("Creates and adds an Entity to the database")]
        public virtual async Task Create(TEntity entity)
        {
            try
            {
                var id = await Service.CreateAsync(entity);

                Console.WriteLine($"{entity.GetType().Name} Created!");
                Console.WriteLine($"Id={id}");
            }
            catch (EntityValidationException e)
            {
                HandleEntityValidationException(e);
            }
        }

        [Description("Updates an Entity in the database")]
        public virtual async Task Update(TEntity entity)
        {
            try
            {
                await Service.UpdateAsync(entity);

                Console.WriteLine($"{entity.GetType().Name} Updated!");
            }
            catch (EntityValidationException e)
            {
                HandleEntityValidationException(e);
            }
        }

        [Description("Gets an Entity from the database")]
        public virtual async Task Get(string id)
        {
            var entity = await Service.GetAsync(id);

            Console.WriteLine(JsonConvert.SerializeObject(entity, Formatting.Indented));
        }

        private static void HandleEntityValidationException(EntityValidationException e)
        {
            Console.WriteLine("Errors:");
            ConsoleEx.WriteLines(e.Errors, (value, i) => $"{i}: ");
        }
    }
}
