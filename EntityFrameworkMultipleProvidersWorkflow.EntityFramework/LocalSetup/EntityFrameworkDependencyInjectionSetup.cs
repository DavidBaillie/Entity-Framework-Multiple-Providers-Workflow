using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Providers;
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
            collection.AddDbContext<TodoItemsDbContext, TodoItemsLocalDbContext>();

            collection.AddSingleton<SqlLiteDatabaseConnectionPersistor>();
            collection.AddTransient<SqlLiteDatabaseInitializer>();

            return collection;
        }

        private static IServiceCollection SetupSqlServerEfProvider(IServiceCollection collection)
        {
            collection.AddDbContext<TodoItemsDbContext, TodoItemsSqlServerDbContext>();

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
