using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskHub.Data;
using TaskHub.Entities;
using TaskHub.Models;

namespace TaskHub.Services
{
    public class TaskDetailService : ITaskDetailService
    {
        private readonly TaskHubContext _context;
        private readonly ILogger<TaskDetailService> _logger;

        public TaskDetailService(TaskHubContext context, ILogger<TaskDetailService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TaskTodoDetail?> GetDetailsOfTask(int taskId)
        {
            _logger.LogDebug("Fetching detail for task with ID: {taskId}", taskId);
            
            return await _context.TaskTodoDetails
                .SingleOrDefaultAsync(t => t.TaskTodo.Id == taskId);
        }

        public async Task<TaskTodoDetail?> CreateDetailsForTask(int taskId, TaskTodoDetailsDto taskTodoDetailsDto)
        {
            _logger.LogDebug("Creating details for task with Id: {taskId}", taskId);

            TaskTodoDetail taskTodoDetail = new(){ 
                Deadline = taskTodoDetailsDto.Deadline,
                Priority = taskTodoDetailsDto.Priority,
                Status = taskTodoDetailsDto.Status,
            };

            var task = await _context.TaskTodos
                .SingleOrDefaultAsync(d => d.Id == taskId);

            if (task == null) 
                return null;

            taskTodoDetail.TaskTodo = task;
            _context.TaskTodoDetails.Add(taskTodoDetail);

            await _context.SaveChangesAsync(); 
            return taskTodoDetail;
        }

        public async Task<TaskTodoDetail?> EditTaskDetails(int taskDetailsId, TaskTodoDetailsDto taskTodoDetailsDto)
        {
            _logger.LogDebug("Editing details of details ID: {taskDetailsId}", taskDetailsId);

            var taskDetails = await _context.TaskTodoDetails
             .SingleOrDefaultAsync(d => d.Id == taskDetailsId);

            if (taskDetails == null)
                return null;

            taskDetails.Deadline = taskTodoDetailsDto.Deadline ?? taskDetails.Deadline;
            taskDetails.Priority = taskTodoDetailsDto.Priority ?? taskDetails.Priority;
            taskDetails.Status = taskTodoDetailsDto.Status ?? taskDetails.Status;

            await _context.SaveChangesAsync();

            return taskDetails;
        }
    }
}
