using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using UserApi.Controllers;
using UserApi.Models.RequestModel;
using UserApi.Models.ResponseModels;
using UserApi.Services;
using Xunit;

namespace UserApi.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockService = new Mock<IUserService>();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(x => x["JwtSettings:Key"]).Returns("YourTestSecretKey1234567890");

            _controller = new UserController(_mockService.Object, configurationMock.Object);
        }

        [Fact]
        public void Login_ShouldReturnToken_WhenUserIsValid()
        {
            // Arrange
            var userLoginRequest = new UserLoginRequestModel { Mail = "testuser", Password = "password123" };
            _mockService.Setup(s => s.ValidateUser(userLoginRequest)).ReturnsAsync(1);

            // Act
            var result = _controller.Login(userLoginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseModel<string>>(okResult.Value);
            Assert.NotNull(response.Data);
            Assert.True(response.Message == null || response.Message == "");
        }

        [Fact]
        public void Login_ShouldReturnError_WhenUserIsInvalid()
        {
            // Arrange
            var userLoginRequest = new UserLoginRequestModel { Mail = "invaliduser", Password = "wrongpassword" };
            _mockService.Setup(s => s.ValidateUser(userLoginRequest)).ReturnsAsync(0);

            // Act
            var result = _controller.Login(userLoginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseModel<string>>(okResult.Value);
            Assert.Null(response.Data);
            Assert.Equal("UserName or Password is incorrect", response.Message);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreatedUser_WhenSuccessful()
        {
            // Arrange
            var userCreateRequest = new UserCreateRequestModel { Email = "testuser", Password = "password123" };
            var userResponseModel = new UserResponseModel { Id = 1, Email = "testuser" };

            _mockService.Setup(s => s.CreateUserAsync(userCreateRequest)).ReturnsAsync(userResponseModel);

            // Act
            var result = await _controller.CreateUser(userCreateRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ResponseModel<UserResponseModel>>(createdResult.Value);
            Assert.NotNull(response.Data);
            Assert.Equal(userResponseModel.Email, response.Data.Email);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_WhenCreationFails()
        {
            // Arrange
            var userCreateRequest = new UserCreateRequestModel { Email = "testuser", Password = "password123" };

            _mockService.Setup(s => s.CreateUserAsync(userCreateRequest)).ReturnsAsync((UserResponseModel)null);

            // Act
            var result = await _controller.CreateUser(userCreateRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ResponseModel<User>>(badRequestResult.Value);
            Assert.Null(response.Data);
            Assert.Equal("Failed to create user.", response.Message);
        }

        [Fact]
        public void GetAllUsers_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new List<UserResponseModel>
            {
                new UserResponseModel { Id = 1, Email = "user1" },
                new UserResponseModel { Id = 2, Email = "user2" }
            };

            _mockService.Setup(s => s.GetAllUsers()).Returns(users);

            // Act
            var result = _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseModel<IEnumerable<UserResponseModel>>>(okResult.Value);
            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.ToList().Count);
        }

        [Fact]
        public void GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var userResponse = new UserResponseModel { Id = userId, Email = "testuser" };

            _mockService.Setup(s => s.GetUserById(userId)).Returns(userResponse);

            // Act
            var result = _controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseModel<UserResponseModel>>(okResult.Value);
            Assert.NotNull(response.Data);
            Assert.Equal(userId, response.Data.Id);
        }

        [Fact]
        public void GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 99;

            _mockService.Setup(s => s.GetUserById(userId)).Returns((UserResponseModel)null);

            // Act
            var result = _controller.GetUserById(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ResponseModel<User>>(notFoundResult.Value);
            Assert.Null(response.Data);
            Assert.Equal("User not found.", response.Message);
        }
    }
}