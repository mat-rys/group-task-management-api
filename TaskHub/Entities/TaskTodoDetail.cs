using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskHub.Entities
{
    public class TaskTodoDetail
    {
        [Key]
        public int Id { get; set; }
        public DateOnly? Deadline { get; set; }
        public string? Status { get; set; }
        public int? Priority { get; set; }
        [JsonIgnore]
        [Required]
        public TaskTodo TaskTodo { get; set; }
    }
}
