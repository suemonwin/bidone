using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserWebApi.Models;
using UserWebApi.Services;
using Xunit;

namespace UserWebApi.Controllers.Tests
{
    public class UserControllerTest
    {
        [Fact]
        public async Task GetUsers_ReturnsOkResult_WhenUsersExist()
        {
            // Arrange
            var userRepositoryMock = new Mock<IDataRepository<User>>();
            userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new[] { new User(), new User() });

            var loggerMock = new Mock<ILogger<UserController>>();
            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<User[]>(okResult.Value);
            Assert.Equal(2, users.Length); // Assuming two users were returned
        }

        [Fact]
        public async Task GetUsers_ReturnsNoContent_WhenNoUsersExist()
        {
            // Arrange
            var userRepositoryMock = new Mock<IDataRepository<User>>();
            userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync((User[])null); // Emulating no users

            var loggerMock = new Mock<ILogger<UserController>>();
            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.GetUsers();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            int userId = 1; // Assuming user ID 1 exists
            var existingUser = new User { Id = userId };

            var userRepositoryMock = new Mock<IDataRepository<User>>();
            userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(existingUser);

            var loggerMock = new Mock<ILogger<UserController>>();
            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<User>(okResult.Value);
            Assert.Equal(userId, user.Id); // Verify the correct user was retrieved
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1; // Assuming user ID 1 does not exist

            var userRepositoryMock = new Mock<IDataRepository<User>>();
            userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);

            var loggerMock = new Mock<ILogger<UserController>>();
            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var existingUser = new User { Id = 1, FirstName = "John", LastName = "Doe" }; // Assuming user ID 1 exists
            var updatedUser = new User { Id = 1, FirstName = "Jane", LastName = "Doe" }; // Updated data

            var userRepositoryMock = new Mock<IDataRepository<User>>();
            userRepositoryMock.Setup(repo => repo.Update(updatedUser)).ReturnsAsync(true);
            userRepositoryMock.Setup(repo => repo.GetById(updatedUser.Id)).ReturnsAsync(existingUser);

            var loggerMock = new Mock<ILogger<UserController>>();
            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.Update(updatedUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedUser.Id, okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var invalidUser = new User { Id = 1, FirstName = "John@", LastName = "Doe$" }; // Invalid data
            var userRepositoryMock = new Mock<IDataRepository<User>>();
            userRepositoryMock.Setup(repo => repo.Update(invalidUser)).ReturnsAsync(false);

            var loggerMock = new Mock<ILogger<UserController>>();
            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.Update(invalidUser);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            // Arrange
            int userId = 1; // Assuming user ID 1 exists
            var userRepositoryMock = new Mock<IDataRepository<User>>();
            userRepositoryMock.Setup(repo => repo.Delete(userId)).ReturnsAsync(true);

            var loggerMock = new Mock<ILogger<UserController>>();
            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.Delete(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userId, okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenDeleteFails()
        {
            // Arrange
            int userId = 1; // Assuming user ID 1 does not exist
            var userRepositoryMock = new Mock<IDataRepository<User>>();
            userRepositoryMock.Setup(repo => repo.Delete(userId)).ReturnsAsync(false);

            var loggerMock = new Mock<ILogger<UserController>>();
            var controller = new UserController(userRepositoryMock.Object, loggerMock.Object);

            // Act
            var result = await controller.Delete(userId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
