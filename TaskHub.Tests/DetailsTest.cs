using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskHub.Controllers;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Tests
{
    public class DetailsTest
    {
        private readonly Mock<ITaskDetailService> _taskDetailMock;
        private readonly TasksDetailsController _controller;

        public DetailsTest()
        {
            _taskDetailMock = new Mock<ITaskDetailService>();
            _controller = new TasksDetailsController(_taskDetailMock.Object);
        }

        private static TaskTodoDetail CreateMockTaskDetail(int taskId) => new()
        {
            Id = taskId,
            Priority = 3,
            Status = "Done",
            Deadline = new DateOnly(2025, 3, 1),
            TaskTodo = new TaskTodo { Id = taskId, Name = "Task", Description = "Task description" }
        };

        [Fact]
        public async Task GetDetailsOfTask_ReturnsOk_WhenDetailsExist()
        {
            // Arrange
            var taskId = 1;
            var taskDetails = CreateMockTaskDetail(taskId);
            _taskDetailMock.Setup(svc => svc.GetDetailsOfTask(taskId)).ReturnsAsync(taskDetails);

            // Act
            var result = await _controller.GetDetailsOfTask(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTaskDetail = Assert.IsType<TaskTodoDetail>(okResult.Value);
            Assert.Equal(taskDetails.Priority, returnedTaskDetail.Priority);
            Assert.Equal(taskDetails.Status, returnedTaskDetail.Status);
            Assert.Equal(taskDetails.Deadline, returnedTaskDetail.Deadline);
            Assert.Equal(taskDetails.TaskTodo.Id, returnedTaskDetail.TaskTodo.Id);
        }

        [Fact]
        public async Task CreateDetailsForTask_ReturnCreatedAtAction_WhenCreatedDetails()
        {
            // Arrange
            var task = new TaskTodo { Id = 1, Name = "new_name", Description = "new_description" };
            var newDetails = new TaskTodoDetailsDto
            {
                Priority = 3,
                Status = "Done",
                Deadline = new DateOnly(2025, 3, 1)
            };
            var taskExpectedResult = CreateMockTaskDetail(task.Id);

            _taskDetailMock.Setup(svc => svc.CreateDetailsForTask(task.Id, newDetails)).ReturnsAsync(taskExpectedResult);

            // Act
            var result = await _controller.CreateDetailsForTask(task.Id, newDetails);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedTaskDetail = Assert.IsType<TaskTodoDetail>(createdAtActionResult.Value);
            Assert.Equal(newDetails.Deadline, returnedTaskDetail.Deadline);
            Assert.Equal(newDetails.Status, returnedTaskDetail.Status);
            Assert.Equal(newDetails.Priority, returnedTaskDetail.Priority);
        }

        [Fact]
        public async Task EditTaskDetails_ReturnOk_WhenDetailsEdited()
        {
            // Arrange
            var id = 1;
            var changedDetails = new TaskTodoDetailsDto
            {
                Priority = 2,
                Status = "Done",
                Deadline = new DateOnly(2025, 3, 1)
            };
            var newDetails = CreateMockTaskDetail(id);
            _taskDetailMock.Setup(svc => svc.EditTaskDetails(id, changedDetails)).ReturnsAsync(newDetails);

            // Act
            var result = await _controller.EditTaskDetails(id, changedDetails);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTaskDetail = Assert.IsType<TaskTodoDetail>(okResult.Value);
            Assert.Equal(newDetails.Deadline, returnedTaskDetail.Deadline);
            Assert.Equal(newDetails.Status, returnedTaskDetail.Status);
            Assert.Equal(newDetails.Priority, returnedTaskDetail.Priority);
        }
    }
}