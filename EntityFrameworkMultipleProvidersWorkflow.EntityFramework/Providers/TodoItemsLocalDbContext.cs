using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Providers
{
    /// <summary>
    /// This class represents the provider that will be used on local developer machines when working on features. This example uses 
    /// SqlLite as the data source, however any database could be used. In this case an in memeory option was selected to allow
    /// developers to immediately start working after cloning the repository. Otherwise they would need a local DBMS and setup scripts
    /// to begin working with a data source.
    /// </summary>
    public class TodoItemsLocalDbContext : TodoItemsDbContext
    {
        public TodoItemsLocalDbContext(DbContextOptions<TodoItemsLocalDbContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// Here we create the test and example data developers will use during their local development. This will not be tracked in Migration and will not be present in 
        /// production and test spaces
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.HasData(
                    new User() { Id = new Guid("975218b8-d68d-4478-b261-6639a180b4e8"), Name = "Catherine Halsey" },
                    new User() { Id = new Guid("875218b8-d68d-4478-b261-6639a180b4e8"), Name = "Miranda Keyes" },
                    new User() { Id = new Guid("775218b8-d68d-4478-b261-6639a180b4e8"), Name = "Avery Johnson" });
            });

            builder.Entity<TodoItem>(entity =>
            {
                entity.HasData(
                    new TodoItem() { Id = new Guid("335218b8-d68d-4478-b261-6639a180b4e8"), UserId = null, Title = "Bake Cake", Description = "With sugar this time." },
                    new TodoItem() { Id = new Guid("225218b8-d68d-4478-b261-6639a180b4e8"), UserId = new Guid("875218b8-d68d-4478-b261-6639a180b4e8"), Title = "Say Please", Description = "It's hard." },
                    new TodoItem() { Id = new Guid("115218b8-d68d-4478-b261-6639a180b4e8"), UserId = new Guid("775218b8-d68d-4478-b261-6639a180b4e8"), Title = "Go to Work", Description = "Rent is due, no choice." });
            });
        }
    }
}
