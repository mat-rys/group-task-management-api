using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Data;
using TaskHub.Data;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksTodoController : ControllerBase
    {
        private readonly ITaskService _task_service;

        public TasksTodoController(ITaskService taskService){
            _task_service = taskService;
        }

        [Authorize(Roles = "LEADER,USER")]
        [HttpGet("{userName}")]
        [EndpointSummary("Get tasks and assigned to them users for a specific user.")]
        [Produces(typeof(IEnumerable<TaskTodo>))] 
        public async Task<IActionResult> GetTasksByUser(string userName){
           
            var tasks = await _task_service.GetTasksByUser(userName);
            return tasks is null
                ? NotFound("No tasks found for this user.")
                : Ok(tasks);
        }

        [Authorize(Roles = "LEADER")]
        [HttpPost]
        [EndpointSummary("Add a new task.")]
        [Produces(typeof(TaskTodo))]
        public async Task<IActionResult> PostNewTask([FromBody] TaskTodoDto newTaskTodo){

            var task = await _task_service.PostNewTask(newTaskTodo);
            return task is null 
                ? BadRequest("Invalid task data")  
                : CreatedAtAction(nameof(PostNewTask), task);
        }

        [Authorize(Roles = "LEADER")]
        [HttpPut("{userName}/{taskId}/assign")]
        [EndpointSummary("Assign a task to a user.")]
        [Produces(typeof(void))]
        public async Task<IActionResult> PutTaskToUser(string userName, int taskId){

            return await _task_service.PutTaskToUser(userName, taskId)
                ? Ok()
                : BadRequest();
        }

        [Authorize(Roles = "LEADER")]
        [HttpPut("{taskId}")]
        [EndpointSummary("Partially update a task.")]
        [Produces(typeof(TaskTodo))]
        public async Task<IActionResult> UpdateTaskPartial(int taskId, [FromBody] TaskTodoDto partialDto){
            
            var task = await _task_service.UpdateTaskPartial(taskId, partialDto);
            return task is null
                  ? BadRequest("No tasks found for this user.")
                  : Ok(task);
        }

        [Authorize(Roles = "LEADER")]
        [HttpDelete("{taskId}/users/{userName}")]
        [EndpointSummary("Remove a task from a user.")]
        [Produces(typeof(void))]
        public async Task<IActionResult> DeleteTaskFromUser(string userName, int taskId){
            
            return await _task_service.DeleteTaskFromUser(userName,taskId)
              ? NoContent()
              : NotFound();
        }

        [Authorize(Roles = "LEADER")]
        [HttpDelete("{taskId}")]
        [EndpointSummary("Delete a task with all its relations.")]
        [Produces(typeof(void))]
        public async Task<IActionResult> DeleteTaskWithAllRelations(int taskId){
            
            return await  _task_service.DeleteTaskWithAllRelations(taskId) 
                ? NoContent()
                : NotFound();
        }
    }
}
