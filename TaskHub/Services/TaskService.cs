
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskHub.Controllers;
using TaskHub.Data;
using TaskHub.Entities;
using TaskHub.Models;

namespace TaskHub.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskHubContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(TaskHubContext context, ILogger<TaskService> logger) { 
            _context = context;
            _logger = logger;
        }

        public async Task<bool> DeleteTaskFromUser(string userName, int taskId){
            _logger.LogDebug("Removing task:{taskid} from user {userName}", userName, taskId);

            var userProfile = await _context.UserProfiles
                .Include(u => u.Tasks)
                .FirstOrDefaultAsync(u => u.UserName == userName);
            if (userProfile == null){
                _logger.LogWarning("Task to delete was not found");
                return false;
            }

            var task = await _context.TaskTodos.FindAsync(taskId);
            if (task == null) return false;
            
            userProfile.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskWithAllRelations(int taskId){
            _logger.LogDebug("Deleting task: {taskId}", taskId);

            var task = await _context.TaskTodos
              .Include(t => t.UserProfiles)
              .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null){
                _logger.LogWarning("Task to delete was not found");
                return false;
            }
               
                
            task.UserProfiles.Clear();
            await _context.SaveChangesAsync();

            _context.TaskTodos.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskTodo>> GetTasksByUser(string userName){
            _logger.LogDebug("Fetching tasks for user: {userName}", userName);

            return await _context.TaskTodos
                .Include(t => t.UserProfiles)
                .Where(t => t.UserProfiles.Any(u => u.UserName == userName))
                .ToListAsync();
        }

        public async Task<TaskTodo?> PostNewTask(TaskTodoDto newTaskTodo){
            _logger.LogDebug("Creating new task: {newTaskTodo}", newTaskTodo);

            if (newTaskTodo == null || string.IsNullOrWhiteSpace(newTaskTodo.Name)){
                _logger.LogWarning("Task creation failed due to invalid data.");
                return null;
            }

            TaskTodo task = new()
            {
                Name = newTaskTodo.Name,
                Description = newTaskTodo.Description
            };

            _context.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> PutTaskToUser(string userName, int taskId){
            _logger.LogDebug("Connecting user {userName} with {taskid}", userName, taskId);

            var userProfile = await _context.UserProfiles
                 .Include(u => u.Tasks)
                 .FirstOrDefaultAsync(u => u.UserName == userName);

            var task = await _context.TaskTodos.FindAsync(taskId);

            if (userProfile == null || task == null){
                _logger.LogWarning("Connection task and user ended with failure");
                return false;
            }
               
            userProfile.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TaskTodo?> UpdateTaskPartial(int id, TaskTodoDto partialDto){
            _logger.LogDebug("Changing task with id: {id}", id);

            var task = await _context.TaskTodos.FindAsync(id);
            if (task == null){
                _logger.LogWarning("Task for update was not found");
                return null;
            }     

            task.Name = partialDto.Name ?? task.Name;
            task.Description = partialDto.Description ?? task.Description;

            await _context.SaveChangesAsync();
            return task;
        }
    }
}
