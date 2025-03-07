using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskHub.Entities
{
    public class TaskComment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public required TaskTodo TaskTodo { get; set; }
        [JsonIgnore]
        public required UserProfile UserProfile { get; set; }  
    }
}
