using Microsoft.AspNetCore.Mvc;
using TaskHub.Entities;
using TaskHub.Models;

namespace TaskHub.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskTodo>> GetTasksByUser(string userName);
        Task<TaskTodo?> PostNewTask(TaskTodoDto newTaskTodo);
        Task<bool> PutTaskToUser(string userName, int taskId);
        Task<bool> DeleteTaskFromUser(string userName, int taskId);
        Task<bool> DeleteTaskWithAllRelations(int taskId);
        Task<TaskTodo?> UpdateTaskPartial(int id, TaskTodoDto partialDto);
    }
}
