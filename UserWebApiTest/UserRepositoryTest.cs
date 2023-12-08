using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserWebApi.Models;
using UserWebApi.Services;
using Xunit;

namespace UserWebApi.Tests.Services
{
    public class UserRepositoryTest
    {
        [Fact]
        public async Task GetAll_ReturnsDataList_WhenJsonDataServiceHasData()
        {
            // Arrange
            var jsonDataServiceMock = new Mock<IJsonDataService>();
            jsonDataServiceMock.Setup(x => x.GetDataFromJasonFile()).ReturnsAsync(new List<User> { new User { Id = 1, FirstName = "John", LastName = "Doe" } });

            var loggerMock = new Mock<ILogger<UserRepository>>();

            var userRepository = new UserRepository(loggerMock.Object, jsonDataServiceMock.Object, Mock.Of<IValidator>());

            // Act
            var result = await userRepository.GetAll();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
            Assert.Equal("John", result.First().FirstName);
            Assert.Equal("Doe", result.First().LastName);
        }
        [Fact]
        public async Task GetById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var jsonDataServiceMock = new Mock<IJsonDataService>();
            jsonDataServiceMock.Setup(x => x.GetDataFromJasonFile()).ReturnsAsync(new List<User> { new User { Id = 1, FirstName = "John", LastName = "Doe" } });

            var loggerMock = new Mock<ILogger<UserRepository>>();

            var userRepository = new UserRepository(loggerMock.Object, jsonDataServiceMock.Object, Mock.Of<IValidator>());

            // Act
            var result = await userRepository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
        }

        [Fact]
        public async Task Create_ReturnsUser_WhenValidationPasses()
        {
            // Arrange
            var jsonDataServiceMock = new Mock<IJsonDataService>();
            jsonDataServiceMock.Setup(x => x.GetDataFromJasonFile()).ReturnsAsync(new List<User>());

            var loggerMock = new Mock<ILogger<UserRepository>>();

            var validatorMock = new Mock<IValidator>();
            validatorMock.Setup(x => x.SpecialCharacterValidator(It.IsAny<string>())).Returns(true);

            var userRepository = new UserRepository(loggerMock.Object, jsonDataServiceMock.Object, validatorMock.Object);
            var newUser = new User { FirstName = "Alice", LastName = "Smith" };

            // Act
            var result = await userRepository.Create(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alice", result.FirstName);
            Assert.Equal("Smith", result.LastName);
        }
        [Fact]
        public async Task Create_ValidUserData_ReturnsUser()
        {
            // Arrange
            var validUserData = new User
            {
                FirstName = "John",
                LastName = "Doe"
                // Add other necessary properties
            };

            var jsonDataServiceMock = new Mock<IJsonDataService>();
            jsonDataServiceMock.Setup(service => service.GetDataFromJasonFile()).ReturnsAsync(new List<User>());

            var loggerMock = new Mock<ILogger<UserRepository>>();
            var userValidatorMock = new Mock<IValidator>();
            userValidatorMock.Setup(validator => validator.SpecialCharacterValidator(It.IsAny<string>())).Returns(true);

            var userRepository = new UserRepository(loggerMock.Object, jsonDataServiceMock.Object, userValidatorMock.Object);

            // Act
            var result = await userRepository.Create(validUserData);

            // Assert
            Assert.NotNull(result);
            // Add assertions to validate the returned user matches the provided validUserData
            // For example:
            Assert.Equal(validUserData.FirstName, result.FirstName);
            Assert.Equal(validUserData.LastName, result.LastName);
        }

        [Fact]
        public async Task Create_InvalidUserData_ReturnsEmptyUser()
        {
            // Arrange
            var invalidUserData = new User
            {
                FirstName = "John@",
                LastName = "Doe$"
                // Add other necessary properties with special characters
            };

            var jsonDataServiceMock = new Mock<IJsonDataService>();
            jsonDataServiceMock.Setup(service => service.GetDataFromJasonFile()).ReturnsAsync(new List<User>());

            var loggerMock = new Mock<ILogger<UserRepository>>();
            var userValidatorMock = new Mock<IValidator>();
            userValidatorMock.Setup(validator => validator.SpecialCharacterValidator(It.IsAny<string>())).Returns(false);

            var userRepository = new UserRepository(loggerMock.Object, jsonDataServiceMock.Object, userValidatorMock.Object);

            // Act
            var result = await userRepository.Create(invalidUserData);

            // Assert
            Assert.NotNull(result);
            // Add assertions to validate that an empty user is returned for invalid data
            // For example:
            Assert.Equal(0, result.Id); // Assuming Id defaults to 0 for an empty user
        }

        [Fact]
        public async Task Update_InvalidUserData_ReturnsFalse()
        {
            // Arrange
            var invalidUserData = new User
            {
                Id = 1, // Assuming there's an existing user with ID 1
                FirstName = "John@",
                LastName = "Doe$"
                // Add other necessary properties with special characters
            };

            var jsonDataServiceMock = new Mock<IJsonDataService>();
            jsonDataServiceMock.Setup(service => service.GetDataFromJasonFile()).ReturnsAsync(new List<User> { new User { Id = 1 } });

            var loggerMock = new Mock<ILogger<UserRepository>>();
            var userValidatorMock = new Mock<IValidator>();
            userValidatorMock.Setup(validator => validator.SpecialCharacterValidator(It.IsAny<string>())).Returns(false);

            var userRepository = new UserRepository(loggerMock.Object, jsonDataServiceMock.Object, userValidatorMock.Object);

            // Act
            var result = await userRepository.Update(invalidUserData);

            // Assert
            Assert.False(result); // Expecting false as the update should fail due to invalid data
            // Add further assertions if needed based on your requirements
        }

        [Fact]
        public async Task Update_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var jsonDataServiceMock = new Mock<IJsonDataService>();
            var loggerMock = new Mock<ILogger<UserRepository>>();
            var validatorMock = new Mock<IValidator>();

            var userList = new List<User>
            {
                new User { Id = 1, FirstName = "John", LastName = "Doe" },
                new User { Id = 2, FirstName = "Alice", LastName = "Smith" }
            };

            jsonDataServiceMock.Setup(x => x.GetDataFromJasonFile()).ReturnsAsync(userList);

            var userRepository = new UserRepository(loggerMock.Object, jsonDataServiceMock.Object, validatorMock.Object);
            var updatedUser = new User { Id = 3, FirstName = "UpdatedName", LastName = "UpdatedLastName" }; // User with ID 3 doesn't exist

            // Act
            var result = await userRepository.Update(updatedUser);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Delete_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var jsonDataServiceMock = new Mock<IJsonDataService>();
            var loggerMock = new Mock<ILogger<UserRepository>>();
            var validatorMock = new Mock<IValidator>();

            var userList = new List<User>
            {
                new User { Id = 1, FirstName = "John", LastName = "Doe" },
                new User { Id = 2, FirstName = "Alice", LastName = "Smith" }
            };

            jsonDataServiceMock.Setup(x => x.GetDataFromJasonFile()).ReturnsAsync(userList);

            var userRepository = new UserRepository(loggerMock.Object, jsonDataServiceMock.Object, validatorMock.Object);

            // Act
            var result = await userRepository.Delete(3); // User with ID 3 doesn't exist

            // Assert
            Assert.False(result);
        }
    }
}
