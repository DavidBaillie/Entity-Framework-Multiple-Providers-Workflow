# Overview
This repository serves as an example project for setting up multiple DbContext providers in a C# .NET project for database access. The primary benefit of this model is to allow a single project to use multiple forms of data access depending on the deployment environment. 
For example, on a developers local machine they may wish to use an in-memory database such as SqlLite for the creation and testing of new features. The CI/CD pipeline might configure a Postgres Database in a Docker container that the application can connect to from another provider, 
and in production a SQL Server instance can be connected to for data persistence. All three modes of testing and persistence can be build and managed via configuration in the application to allow easier scaling and flexibility of data source at all stages of the development process.

# How it Works
The typical Entity Framework workflow is to define a single Database Context (DbContext) per database the developer would like to connect to and then use the DbContext as a "Unit of Work" when manipulating the data persistence layer. Microsoft allows for the creation of "providers" that are classes which 
inherit from a parent dbContext. The parent dbContext is able to define the tables and relationships that must be present in the data layer, while the inheriting classes can define the source and starting data that goes with the source. Thus, using Dependency Injection we are able to inject one of the providers into 
the Container under the typeof the parent dbContext class. 

In this example project, the TodoListDbContext acts as the base class:
```
public abstract class TodoItemsDbContext : DbContext
{
    public DbSet<TodoItem> Items { get; set; }
    public DbSet<User> Users { get; set; }

    public TodoItemsDbContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Table definitions here...
    }
}
```

The inheriting classes from this dbContext will be the "providers" for the dbContext when called in other classes. 

```
public class TodoItemsSqlLiteDbContext : TodoItemsDbContext
{
    public TodoItemsLocalDbContext(DbContextOptions<TodoItemsLocalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    
        builder.Entity<TodoItem>(entity =>
        {
            entity.HasData(
                new TodoItem() { Id = new Guid("335218b8-d68d-4478-b261-6639a180b4e8"), UserId = null, Title = "Bake Cake", Description = "With sugar this time." },
                new TodoItem() { Id = new Guid("225218b8-d68d-4478-b261-6639a180b4e8"), UserId = new Guid("875218b8-d68d-4478-b261-6639a180b4e8"), Title = "Say Please", Description = "It's hard." },
                new TodoItem() { Id = new Guid("115218b8-d68d-4478-b261-6639a180b4e8"), UserId = new Guid("775218b8-d68d-4478-b261-6639a180b4e8"), Title = "Go to Work", Description = "Rent is due, no choice." });
        });
    }
}
```

As we can see the SqlLite provider has also started the database with some seeded data in the form of Todo Items in the Items table. This information will only be present when using the SqlLite provider and not when using any other provider.

# Migrations
An important part of any Entity Framework workflow is the ability to use Migrations for Up and Down changes to the database in deployment spaces. Typically a single dbContext would handle all migrations for the project and be automatically selected when running migration commands. Because we are using multiple providers in this 
case, we will instead need to specify which provider the migration will be performed on by the tools. In the above example, the SqlLite implementation will never be deployed to a user since it's assumed to be in-memory for developers. Instead we will add another provider to use SQL Server which will be used during deployment to our 
cloud resources.

```
public class TodoItemsSqlServerDbContext : TodoItemsDbContext
{
    public TodoItemsSqlServerDbContext(DbContextOptions<TodoItemsSqlServerDbContext> options) : base(options) { }
}
```

No additional configuration is done on the provider itself as that will be handled during the Dependency Injection process later. Should you need seed/start data to exist in your deployed environments, you would add it here in the same way the seeded data was added in the SqlLite example. Understand that anything seeded 
here will be tracked in migrations.

To start migrations with this provider, we use the following command:

```
Add-Migration YourMessageHere -Context TodoItemsSqlServerDbContext
```

This will generate a new migration with our changes that can then be used to Upgrade or Downgrade the deployed SQL Server instance we use in our production space. 

# Configuration
Now that all the needed code is in place for different providers, we will need to configure it in the startup of our project. An extension has been provided in this repository to keep things simple:

```
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
```

In the above methods, the public extension (first one) looks at the configuration value for the provider and calls the correct setup method based on that. When we are using "local", the Container is provided SqlLite implementation and some supporting classes to keep the connection alive. 
When we are using "sql", the Container is given the SQL Server provider to use. Now, depending on the environment configuration variables, the Entity Framework class we use (TodoItemsDbContext), can be used anywhere in the project without concern for where the information is coming from. 

# In-Memory SQL Lite
Note the supporting classes seen above, SqlLiteDatabaseConnectionPersistor and SqlLiteDatabaseInitializer. When an in-memory database is used by Entity Framework, no database exists until it is setup by the provider. In this case the SqlLiteDatabaseInitializer class is used to tell Entity Framework to 
setup the database tables and values, whereas SqlLiteDatabaseConnectionPersistor will open a connection the in-memory database and keep the connection alive until the application shuts down. This is done because when using SqlLite in-memory, after the last connection closes the process will terminate all data 
created as part of the cleanup process. By keeping a connection open, the data will not be wiped until the application closes. 
