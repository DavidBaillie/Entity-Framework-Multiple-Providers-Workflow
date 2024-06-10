namespace EntityFrameworkMultipleProvidersWorkflow.EntityFramework.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; } = null;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation Property
        public virtual User? User { get; set; }
    }
}
