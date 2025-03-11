using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using TaskHub.Data;
using TaskHub.Entities;
using TaskHub.Models;

namespace TaskHub.Services
{
    public class TaskCommentService : ITaskCommentService
    {
        private readonly TaskHubContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskCommentService(TaskHubContext taskHubContext, ILogger<TaskService> logger)
        {
            _context = taskHubContext;
            _logger = logger;
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            _logger.LogDebug("Deleting comment: {commentId}", commentId);

            var comment = await _context.TaskComments
           .SingleOrDefaultAsync(d => d.Id == commentId);

            if (comment == null)
            {
                _logger.LogWarning("Comment to delete was not found");
                return false;
            }

            _context.TaskComments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskComment?> GetCommentById(int commentId)
        {
            _logger.LogDebug("Fetching comment with id: {commentId}", commentId);
            return await _context.TaskComments
                .SingleOrDefaultAsync(d => d.Id == commentId);
        }

        public async Task<IEnumerable<TaskComment>> GetCommentsByTask(int taskId)
        {
            _logger.LogDebug("Fetching comments for task with id: {taskId}", taskId);
            return await _context.TaskComments
               .Include(t => t.TaskTodo)
               .Where(t => t.TaskTodo.Id == taskId)
               .ToListAsync();
        }

        public async Task<TaskComment?> PostNewComment(int taskId, string userId, TaskCommentDto taskCommentDto)
        {
            _logger.LogDebug("Creating new task: {taskCommentDto}", taskCommentDto);
            var task = await _context.TaskTodos
               .SingleOrDefaultAsync(d => d.Id == taskId);

            var user = await _context.UserProfiles
             .SingleOrDefaultAsync(d => d.UserId.Equals(userId));

            if (task == null || user == null) { 
                return null;
            }

            TaskComment taskComment = new()
            {
                Content = taskCommentDto.Content,
                TaskTodo = task,
                UserProfile = user
            };

            _context.Add(taskComment);
            await _context.SaveChangesAsync();

            return taskComment;
        }

        public async Task<TaskComment?> UpdateComment(int commentId, TaskCommentDto taskCommentDto)
        {
            _logger.LogDebug("Updating comment with id: {commentId}", commentId);
            var comment = await _context.TaskComments
                .SingleOrDefaultAsync(d => d.Id == commentId);
            
            if (comment == null)
            {
                _logger.LogWarning("Comment for update was not found");
                return null;
            }

            comment.Content = taskCommentDto.Content ?? comment.Content;
            await _context.SaveChangesAsync();

            return comment;
        }
    }
}
