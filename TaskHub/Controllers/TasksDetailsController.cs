﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskHub.Data;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Controllers
{
    [Route("api/taskDetails")]
    [ApiController]
    public class TasksDetailsController : ControllerBase
    {
        private readonly ITaskDetailService _taskTodoService;

        public TasksDetailsController(ITaskDetailService taskTodoService){
            _taskTodoService = taskTodoService;
        }

        //get Taskdetails by foreignkey taskId
        [Authorize(Roles = "LEADER,USER")]
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetDetailsOfTask(int taskId) {
           
            if (taskId <= 0)
                return BadRequest("Invalid task ID.");
            
            var details = await _taskTodoService.GetDetailsOfTask(taskId);

            return details is null 
                ? NotFound($"Task with ID {taskId} not found.") 
                : Ok(details);
        }

        //post task details
        [HttpPost("{taskId}")]
        [Authorize(Roles = "LEADER")]
        public async Task<IActionResult> CreateDetailsForTask(int taskId, [FromBody] TaskTodoDetailsDto taskTodoDetailsDto)
        {
            if (taskId <= 0 || taskTodoDetailsDto == null)
                return BadRequest("Invalid information");
            
            var taskDetail = await _taskTodoService.CreateDetailsForTask(taskId, taskTodoDetailsDto);

            return taskDetail is null
              ? BadRequest("Invalid task data")
              : CreatedAtAction(nameof(CreateDetailsForTask), taskDetail);
        }

        //put task details
        [HttpPut("{taskDetailsId}")]
        [Authorize(Roles = "LEADER")]
        public async Task<IActionResult> EditTaskDetails(int taskDetailsId, [FromBody] TaskTodoDetailsDto taskTodoDetailsDto)
        {
            if (taskDetailsId <= 0 || taskTodoDetailsDto == null)
                return BadRequest("Invalid information");

            var task = await _taskTodoService.EditTaskDetails(taskDetailsId, taskTodoDetailsDto);
            return task is null
                  ? BadRequest("No details found.")
                  : Ok(task);
        }
    }
}
