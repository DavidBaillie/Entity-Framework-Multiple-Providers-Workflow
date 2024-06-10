using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Models;
using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Providers;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkMultipleProvidersWorkflow.Api.Repositories
{
    public class ItemsRepository : IAsyncRepository<TodoItem>
    {
        private readonly TodoItemsDbContext context;

        public ItemsRepository(TodoItemsDbContext context)
        {
            this.context = context;
        }

        public async Task<TodoItem?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Items.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<TodoItem> CreateAsync(TodoItem entity, CancellationToken cancellationToken = default)
        {
            var dbEntry = await context.Items.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return dbEntry.Entity;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem entity, CancellationToken cancellationToken = default)
        {
            var dbEntry = context.Items.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
            return dbEntry.Entity;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await context.Items.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
