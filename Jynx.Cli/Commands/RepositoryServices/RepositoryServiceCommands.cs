using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Newtonsoft.Json;

namespace Jynx.Cli.Commands.RepositoryServices
{
    internal abstract class RepositoryServiceCommands<TService, TEntity> : ConsoleAppBase
        where TService : IRepositoryService<TEntity>
        where TEntity : BaseEntity
    {
        public RepositoryServiceCommands(TService service)
        {
            Service = service;
        }

        public TService Service { get; }

        [Description("Creates and adds an Entity to the database")]
        public virtual async Task Create(TEntity entity)
        {
            var id = await Service.CreateAsync(entity);

            Console.WriteLine($"{entity.GetType().Name} Created!");
            Console.WriteLine($"Id={id}");
        }

        [Description("Updates an Entity in the database")]
        public virtual async Task Update(TEntity entity)
        {
            await Service.UpdateAsync(entity);

            Console.WriteLine($"{entity.GetType().Name} Updated!");
        }

        [Description("Gets an Entity from the database")]
        public virtual async Task Get(string id)
        {
            var entity = await Service.GetAsync(id);

            Console.WriteLine(JsonConvert.SerializeObject(entity, Formatting.Indented));
        }
    }
}
