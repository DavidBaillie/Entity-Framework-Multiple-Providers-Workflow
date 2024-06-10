using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkMultipleProvidersWorkflow.EntityFramework.LocalSetup
{
    public static class EntityFrameworkDependencyInjectionSetup
    {
        public static IServiceCollection SetupEFDependencies(this IServiceCollection collection, IConfiguration configuration)
        {
            _ = configuration.GetSection("Persistence")["EF_Provider"] switch
            {
                "local" => SetupLocalEfProvider(collection),
                "sql" => SetupSqlServerEfProvider(collection),
                _ => throw new ApplicationException("No EF Provider selected!")
            };

            return collection;
        }

        private static IServiceCollection SetupLocalEfProvider(IServiceCollection collection)
        {
            collection.AddDbContext<TodoItemsDbContext, TodoItemsLocalDbContext>(options =>
            {
                options.UseSqlite("DataSource=file::memory:?cache=shared");
            });

            collection.AddSingleton<SqlLiteDatabaseConnectionPersistor>();
            collection.AddTransient<SqlLiteDatabaseInitializer>();

            return collection;
        }

        private static IServiceCollection SetupSqlServerEfProvider(IServiceCollection collection)
        {
            collection.AddDbContext<TodoItemsDbContext, TodoItemsSqlServerDbContext>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TodoListDatabase;MultipleActiveResultSets=True;Trusted_Connection=True");
            });

            return collection;
        }

        public static IServiceScope SetupEfProviderInApplication(this IServiceScope scope, IConfiguration configuration)
        {
            if (configuration.GetSection("Persistence")["EF_Provider"] == "local")
            {
                scope.ServiceProvider.GetRequiredService<SqlLiteDatabaseInitializer>();
                scope.ServiceProvider.GetRequiredService<SqlLiteDatabaseConnectionPersistor>();
            }

            return scope;
        }
    }
}
