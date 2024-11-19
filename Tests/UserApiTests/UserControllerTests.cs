using Microsoft.AspNetCore.Mvc;
using Moq;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<IUserService> _mockService;

    public UserControllerTests()
    {
        _mockService = new Mock<IUserService>();
        _controller = new UserController(_mockService.Object);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOkResult_WithListOfUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = 1, Name = "User1" },
            new() { Id = 2, Name = "User2" }
        };
        _mockService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnUsers = Assert.IsType<List<User>>(okResult.Value);
        Assert.Equal(2, returnUsers.Count);
    }

    [Fact]
    public async Task GetUserById_UserExists_ReturnsOkResult()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1" };
        _mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetUserById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(user.Id, returnUser.Id);
    }

    [Fact]
    public async Task GetUserById_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.GetUserById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var user = new User { Id = 1, Name = "New User" };
        _mockService.Setup(s => s.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(user);

        // Act
        var result = await _controller.CreateUser(user);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnUser = Assert.IsType<User>(createdAtActionResult.Value);
        Assert.Equal(user.Id, returnUser.Id);
    }

    [Fact]
    public async Task UpdateUser_UserExists_ReturnsNoContent()
    {
        // Arrange
        _mockService.Setup(s => s.UpdateUserAsync(1, It.IsAny<User>())).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateUser(1, new User());

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateUser_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.UpdateUserAsync(1, It.IsAny<User>())).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateUser(1, new User());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteUser_UserExists_ReturnsNoContent()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteUser(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteUser_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteUser(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}