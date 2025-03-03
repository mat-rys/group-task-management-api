using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using TaskHub.Controllers;
using TaskHub.Data;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Tests
{
    public class TasksTest
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly TasksTodoController _controller;

        public TasksTest()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _controller = new TasksTodoController(_taskServiceMock.Object);
        }

        [Fact]
        public async Task GetTasksByUser_ReturnsOk_WhenTasksExist()
        {
            //Arrange
            string userName = "testuser";
            var tasks = new List<TaskTodo>{
                new() { Id = 1, Name = "Task 1", UserProfiles = [new UserProfile { UserName = userName }] },
                new() { Id = 2, Name = "Task 2", UserProfiles = [new UserProfile { UserName = userName }] }
            };
            _taskServiceMock.Setup(svc => svc.GetTasksByUser(userName))
                            .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTasksByUser(userName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsType<List<TaskTodo>>(okResult.Value);
            Assert.Equal(2, returnedTasks.Count());
        }

        [Fact]
        public async Task PostNewTask_ReturnCreatedAtAction_WhenTaskCreated()
        {
            //Arrange
            var taskSended = new TaskTodoDto(){Name = "new_name",Description = "new_description"};
            var taskCreated = new TaskTodo { Id = 1, Name = "new_name", Description = "new_description"};

            _taskServiceMock.Setup(svc => svc.PostNewTask(taskSended))
              .ReturnsAsync(taskCreated);

            //Act
            var result = await _controller.PostNewTask(taskSended);

            //Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedTask = Assert.IsType<TaskTodo>(createdAtActionResult.Value);

            Assert.Equal(taskSended.Name, returnedTask.Name);
            Assert.Equal(taskSended.Description, returnedTask.Description);
        }

        [Fact]
        public async Task UpdateTaskPartial_ReturnOkObject_WhenEdited()
        {
            //Arrange
            var taskUpdate = new TaskTodoDto(){Name = "new_name", Description = "new_description"};
            var taskUpdated = new TaskTodo(){Id = 1, Name = "new_name", Description = "new_description"};

            _taskServiceMock.Setup(svc => svc.UpdateTaskPartial(1, taskUpdate))
                .ReturnsAsync(taskUpdated);

            // Act
            var result = await _controller.UpdateTaskPartial(1, taskUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTask = Assert.IsType<TaskTodo>(okResult.Value);

            Assert.Equal(taskUpdate.Name, returnedTask.Name);
            Assert.Equal(taskUpdate.Description, returnedTask.Description);

        }

        [Fact]
        public async Task DeleteTaskWithAllRelations_ReturnNoContent_WhenTaskDeleted()
        {
            //Arrange
            _taskServiceMock.Setup(svc => svc.DeleteTaskWithAllRelations(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTaskWithAllRelations(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTaskFromUser_ReturnNoContent_WhenRelationDeleted()
        {
            //Arrange
            _taskServiceMock.Setup(svc => svc.DeleteTaskFromUser("user",1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTaskFromUser("user",1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
