
using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Providers;

namespace EntityFrameworkMultipleProvidersWorkflow.EntityFramework.LocalSetup
{
    public class SqlLiteDatabaseInitializer
    {
        public SqlLiteDatabaseInitializer(TodoItemsDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
