using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskHub.Entities
{
    [Index(nameof(UserName),IsUnique = true)]
    public class UserProfile
    {
        [Key]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        [JsonIgnore]
        public List<TaskTodo>? Tasks { get; set; }
        [JsonIgnore]
        public List<TaskComment>? TaskComments { get; set; }

    }
}
