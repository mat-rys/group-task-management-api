namespace TaskHub.Models
{
    public class TaskTodoDetailsDto
    {
        public DateOnly? Deadline { get; set; } = null;
        public string? Status { get; set; } = string.Empty;
        public int? Priority { get; set; } = 0;
    }
}
