using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Controllers;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Tests
{
    public class CommentsTest
    {
        private readonly Mock<ITaskCommentService> _taskCommentService;
        private readonly TaskCommentController _controller;

        public CommentsTest() { 
            _taskCommentService = new Mock<ITaskCommentService>();
            _controller = new TaskCommentController(_taskCommentService.Object);
        }

        [Fact]
        public async Task DeleteComment_ReturnNoContent_WhenCommentDeleted()
        {
            //Arrange
            _taskCommentService.Setup(svc => svc.DeleteComment(1))
                .ReturnsAsync(true);
            //Act
            var result  = await _controller.DeleteComment(1);
            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetCommentById_ReturnTask_WhenCommentFetchedProperly()
        {
            //Arrange
            TaskComment taskComment = new(){Id = 1, Content = "getComment", CreatedAt = DateTime.Now};

            _taskCommentService.Setup(svc => svc.GetCommentById(taskComment.Id))
                .ReturnsAsync(taskComment);

            // Act
            var result = await _controller.GetCommentById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultComment = Assert.IsType<TaskComment>(okResult.Value);
            Assert.Equal("getComment", resultComment.Content);
        }

        [Fact]
        public async Task GetCommentsByTask_ReturnCommentsList_WhenCommentsFetchProperly()
        {
            //Arrange
            int taskId = 1;
            var comments = new List<TaskComment>{
                new(){Id = 1, Content = "Comment1", CreatedAt = DateTime.Now},
                new(){Id = 2, Content = "Comment2", CreatedAt = DateTime.Now} 
            };

            _taskCommentService.Setup(svc => svc.GetCommentsByTask(taskId))
                .ReturnsAsync(comments);
            //Act
            var resultListComments = await _controller.GetCommentsByTask(taskId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(resultListComments);
            var resultComment = Assert.IsType<List<TaskComment>>(okResult.Value);
            Assert.Equal(2, resultComment.Count());
        }

        [Fact]
        public async Task UpdateComment_ReturnOkObjectAndComment_WhenContentInCommentChanged()
        {
            //Arrange
            int taskId = 1;
            TaskCommentDto contentSendedToUpdate = new() {Content = "after"};
            TaskComment taskCommentReturned = new() { Id = 1, Content = "after", CreatedAt= DateTime.Now};

            _taskCommentService.Setup(svc => svc.UpdateComment(taskId, contentSendedToUpdate))
               .ReturnsAsync(taskCommentReturned);
            
            //Act
            var result = await _controller.UpdateTContent(taskId, contentSendedToUpdate);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedComment = Assert.IsType<TaskComment>(okResult.Value);

            Assert.Equal(contentSendedToUpdate.Content, returnedComment.Content);
        }

        [Fact]
        public async Task PostNewCommentForTask_ReturnCreatedAtAction_WhenCommentCreated()
        {
            TaskCommentDto contentSendedToUpdate = new() { Content = "NewComment" };
            TaskComment taskComment = new() {
                Id = 1,
                Content = contentSendedToUpdate.Content,
                CreatedAt = DateTime.Now,
                TaskTodo = new() { Id = 1, Description = "task"},
                UserProfile = new() { UserId = "aaa-222-111", Role = "LEADER", UserName = "mmm" }
            };

            _taskCommentService.Setup(svc => svc.PostNewComment(taskComment.TaskTodo.Id,
                taskComment.UserProfile.UserId,
                contentSendedToUpdate))
                .ReturnsAsync(taskComment);

            //Act
            var result = await _controller.PostNewCommentForTask(taskComment.TaskTodo.Id,
                taskComment.UserProfile.UserId,
                contentSendedToUpdate);

            //Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedComment = Assert.IsType<TaskComment>(okResult.Value);

            Assert.Equal(contentSendedToUpdate.Content, returnedComment.Content);
        }
    }
}
