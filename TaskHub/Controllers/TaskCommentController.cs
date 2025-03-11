using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskHub.Data;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class TaskCommentController : ControllerBase
    {
        private readonly TaskCommentService _taskCommentService;

        public TaskCommentController(TaskCommentService taskCommentService)
        {
            _taskCommentService = taskCommentService;
        }

        [HttpPost("task/{taskId}/user/{userId}")]
        public async Task<IActionResult> PostNewCommentForTask(int taskId, string userId, [FromBody] TaskCommentDto taskCommentDto)
        {
            var taskComment = await _taskCommentService.PostNewComment(taskId, userId,taskCommentDto);
            return taskComment is null
            ? BadRequest("Invalid task data")
            : CreatedAtAction(nameof(PostNewCommentForTask), taskComment);
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            var comment = await _taskCommentService.GetCommentById(commentId);
            return comment is null
               ? NotFound("No tasks found for this user.")
               : Ok(comment);
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<TaskComment>>> GetCommentsByTask(int taskId)
        {
            var comments = await _taskCommentService.GetCommentsByTask(taskId);
            return comments is null
              ? NotFound("No tasks found for this user.")
              : Ok(comments);
        }


        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            return await _taskCommentService.DeleteComment(commentId)
              ? NoContent()
              : NotFound();
        }


        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateTContent(int commentId, [FromBody] TaskCommentDto taskCommentDto)
        {
            var updatedComment = await _taskCommentService.UpdateComment(commentId, taskCommentDto);
            return updatedComment is null
                 ? BadRequest("No tasks found for this user.")
                 : Ok(updatedComment);
        }
    }
}
