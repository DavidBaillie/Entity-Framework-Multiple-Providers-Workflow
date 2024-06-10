using EntityFrameworkMultipleProvidersWorkflow.Api.Repositories;
using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.LocalSetup;
using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Models;

namespace EntityFrameworkMultipleProvidersWorkflow.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.SetupEFDependencies(builder.Configuration);
            builder.Services.AddScoped<IAsyncRepository<TodoItem>, ItemsRepository>();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                scope.SetupEfProviderInApplication(app.Configuration);
            }

            app.Run();
        }
    }
}
