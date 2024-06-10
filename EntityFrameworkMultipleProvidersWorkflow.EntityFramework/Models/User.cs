namespace EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation Property
        public List<TodoItem> TodoItems { get; set; }
    }
}
