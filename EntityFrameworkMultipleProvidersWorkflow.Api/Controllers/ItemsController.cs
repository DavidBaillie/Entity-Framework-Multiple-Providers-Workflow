using EntityFrameworkMultipleProvidersWorkflow.Api.Repositories;
using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkMultipleProvidersWorkflow.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IAsyncRepository<TodoItem> repository;

        public ItemsController(IAsyncRepository<TodoItem> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await repository.GetAsync(id, cancellationToken);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(TodoItem item, CancellationToken cancellationToken)
        {
            var dbItem = await repository.CreateAsync(item, cancellationToken);
            return Ok(dbItem);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(TodoItem item, CancellationToken cancellationToken)
        {
            var dbItem = await repository.UpdateAsync(item, cancellationToken);
            return Ok(dbItem);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await repository.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
