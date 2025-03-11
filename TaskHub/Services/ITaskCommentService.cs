using Microsoft.AspNetCore.Mvc;
using TaskHub.Entities;
using TaskHub.Models;

namespace TaskHub.Services
{
    public interface ITaskCommentService
    {
        Task<TaskComment?> PostNewComment(int taskId, string userId, TaskCommentDto taskCommentDto);
        Task<TaskComment?> GetCommentById(int commentId);
        Task<IEnumerable<TaskComment>> GetCommentsByTask(int taskId);
        Task<bool> DeleteComment(int commentId);
        Task<TaskComment?> UpdateComment(int commentId, TaskCommentDto taskCommentDto);
    }
}
