using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Providers
{
    /// <summary>
    /// This class represent the deployed instance of the API to the cloud with a backing SQL Server instance to store data. 
    /// We don't add the same example data here as anything in this provider will be added to the Migrations folder and thus
    /// will show up when we deploy. 
    /// </summary>
    public class TodoItemsSqlServerDbContext : TodoItemsDbContext
    {
        public TodoItemsSqlServerDbContext(DbContextOptions<TodoItemsSqlServerDbContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// Any entries added here will be tracked as part of the production release to our SQL Server instance and will be in the 
        /// migrations history.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
