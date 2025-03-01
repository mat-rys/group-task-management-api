using Microsoft.AspNetCore.Mvc;
using TaskHub.Entities;
using TaskHub.Models;

namespace TaskHub.Services
{
    public interface ITaskDetailService
    {
        Task<TaskTodoDetail?> GetDetailsOfTask(int taskId);
        Task<TaskTodoDetail?> CreateDetailsForTask(int taskId, TaskTodoDetailsDto taskTodoDetailsDto);
        Task<TaskTodoDetail?> EditTaskDetails(int taskDetailsId, TaskTodoDetailsDto taskTodoDetailsDto);
    }
}
