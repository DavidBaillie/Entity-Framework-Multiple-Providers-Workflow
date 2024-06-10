using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Providers;

namespace EntityFrameworkMultipleProvidersWorkflow.EntityFramework.LocalSetup
{
    public class TodoItemsSqlServerDbContextDesignTimeFactory : IDesignTimeDbContextFactory<TodoItemsSqlServerDbContext>
    {
        public TodoItemsSqlServerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoItemsSqlServerDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TodoListDatabase;MultipleActiveResultSets=True;Trusted_Connection=True");

            return new TodoItemsSqlServerDbContext(optionsBuilder.Options);
        }
    }
}
