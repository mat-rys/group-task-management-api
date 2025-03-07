using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskHub.Entities
{
    public class TaskTodo
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? TaskDetailId { get; set; }
        public TaskTodoDetail? TaskDetail { get; set; }
        public List<UserProfile>? UserProfiles { get; set; }
        public List<TaskComment>? TaskComments { get; set; }
    }
}
