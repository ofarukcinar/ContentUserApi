using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserApi.Controllers;
using UserApi.Models;
using UserApi.Models.RequestModel;
using UserApi.Models.ResponseModels;
using UserApi.Services;
using Xunit;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockService = new Mock<IUserService>();
        _controller = new UserController(_mockService.Object);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOkResult_WithListOfUsers()
    {
        // Arrange
        var users = new List<User> { new User { Id = 1, Name = "Test User" } };
        _mockService.Setup(s => s.GetAllUsers()).ReturnsAsync(users);

        // Act
        var result = _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<IEnumerable<User>>>(okResult.Value);
        Assert.Equal(users, response.Data);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedResult_WhenUserIsCreated()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        var controller = new UserController(mockService.Object);

        var userRequest = new UserCreateRequestModel
        {
            Name = "John Doe",
            Email = "johndoe@example.com"
        };

        var createdUser = new User
        {
            Id = 1,
            Name = "John Doe",
            Email = "johndoe@example.com"
        };

        mockService
            .Setup(s => s.CreateUserAsync(userRequest))
            .ReturnsAsync(createdUser);

        // Act
        var result = await controller.CreateUser(userRequest);

        // Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result);
        var responseModel = Assert.IsType<ResponseModel<User>>(actionResult.Value);
        Assert.True(responseModel.Success);
        Assert.NotNull(responseModel.Data);
        Assert.Equal(createdUser.Id, responseModel.Data.Id);
        Assert.Equal(createdUser.Name, responseModel.Data.Name);
        Assert.Equal(createdUser.Email, responseModel.Data.Email);
    }
    
    [Fact]
    public async Task GetUserById_ReturnsOkResult_WithUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User" };
        _mockService.Setup(s => s.GetUserById(1)).ReturnsAsync(user);

        // Act
        var result = _controller.GetUserById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<User>>(okResult.Value);
        Assert.Equal(user, response.Data);
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetUserById(1)).ReturnsAsync((User)null);

        // Act
        var result = _controller.GetUserById(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseModel<User>>(notFoundResult.Value);
        Assert.Null(response.Data);
    }

    [Fact]
    public async Task UpdateUser_ReturnsOkResult_WhenSuccessful()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Updated User" };
        _mockService.Setup(s => s.UpdateUserAsync(1, user)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateUser(1, user);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(okResult.Value);
        Assert.True(response.Data);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Updated User" };
        _mockService.Setup(s => s.UpdateUserAsync(1, user)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateUser(1, user);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(notFoundResult.Value);
        Assert.False(response.Data);
    }

    [Fact]
    public async Task DeleteUser_ReturnsOkResult_WhenSuccessful()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteUser(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(okResult.Value);
        Assert.True(response.Data);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteUser(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(notFoundResult.Value);
        Assert.False(response.Data);
    }
}