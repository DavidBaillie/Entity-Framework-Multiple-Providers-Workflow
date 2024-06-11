using EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Providers
{
    /// <summary>
    /// This database context is the one used by all classes in the project to access the data persistence layer. Providers inherit from this 
    /// context and provide the exact implementation of persistence. Anything in this class will be applied to all providers.
    /// </summary>
    public abstract class TodoItemsDbContext : DbContext
    {
        public DbSet<TodoItem> Items { get; set; }
        public DbSet<User> Users { get; set; }

        public TodoItemsDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Title).HasMaxLength(100).IsRequired();
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.HasOne(x => x.User)
                    .WithMany(x => x.TodoItems)
                    .HasForeignKey(x => x.UserId);
            });

            builder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
                entity.HasMany(x => x.TodoItems)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }
    }
}
