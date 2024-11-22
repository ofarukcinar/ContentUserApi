using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using UserApi.Controllers;
using UserApi.Models.RequestModel;
using UserApi.Models.ResponseModels;
using UserApi.Services;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IUserService> _mockUserService;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockConfiguration = new Mock<IConfiguration>();

        // JWT Secret Setup (Key with at least 128 bits or 16 characters)
        _mockConfiguration.Setup(c => c["JwtSettings:Key"]).Returns("SuperSecretKey12!2234");

        _controller = new UserController(_mockUserService.Object, _mockConfiguration.Object);
    }


    [Fact]
    public void Login_ShouldReturnOk_WhenCredentialsAreInvalid()
    {
        // Arrange
        var invalidLoginRequest = new UserLoginRequestModel { Mail = "testuser", Password = "wrongpassword" };
        _mockUserService.Setup(s => s.ValidateUser(invalidLoginRequest))
            .ReturnsAsync(0); // Simulate invalid user ID

        // Act
        var result = _controller.Login(invalidLoginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<string>>(okResult.Value);
        Assert.Equal("", response.Data); // Token should be empty for invalid credentials
        Assert.Equal("UserName or Password is incorrect", response.Message);
    }

    [Fact]
    public void GetAllUsers_ShouldReturnOk_WithUserList()
    {
        // Arrange
        var mockUsers = new List<UserResponseModel>
        {
            new() { Id = 1, Email = "newuser@newUser.com" },
            new() { Id = 2, Email = "newuser2@newUser.com" }
        };
        _mockUserService.Setup(s => s.GetAllUsers())
            .Returns(mockUsers);

        // Act
        var result = _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<IEnumerable<UserResponseModel>>>(okResult.Value);
        Assert.Equal(2, response.Data.Count());
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedAtAction_WhenUserIsCreated()
    {
        // Arrange
        var newUserRequest = new UserCreateRequestModel { Email = "newuser@newUser.com", Password = "password" };
        var createdUser = new UserResponseModel
            { Id = 3, Email = "newuser@newUser.com" }; // Match with newUserRequest.Email

        _mockUserService.Setup(s => s.CreateUserAsync(newUserRequest))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _controller.CreateUser(newUserRequest);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<ResponseModel<UserResponseModel>>(createdResult.Value);
        Assert.Equal(newUserRequest.Email, response.Data.Email); // Compare with expected email
    }


    [Fact]
    public async Task CreateUser_ShouldReturnBadRequest_WhenUserCreationFails()
    {
        // Arrange
        var newUserRequest = new UserCreateRequestModel
        {
            Email = "newuser@newUser.com",
            Password = "password",
            Address = "test",
            RoleId = 2
        };

        _mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<UserCreateRequestModel>()))
            .ReturnsAsync((UserResponseModel)null);

        // Act
        var result = await _controller.CreateUser(newUserRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ResponseModel<User>>(badRequestResult.Value);
        Assert.Equal("Failed to create user.", response.Message);
    }


    [Fact]
    public void GetUserById_ShouldReturnOk_WhenUserExists()
    {
        // Arrange
        var user = new UserResponseModel { Id = 1, Email = "newuser@newUser.com" };
        _mockUserService.Setup(s => s.GetUserById(1))
            .Returns(user);

        // Act
        var result = _controller.GetUserById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<UserResponseModel>>(okResult.Value);
        Assert.Equal("newuser@newUser.com", response.Data.Email);
    }

    [Fact]
    public void GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetUserById(5))
            .Returns((UserResponseModel)null);

        // Act
        var result = _controller.GetUserById(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseModel<User>>(notFoundResult.Value);
        Assert.Equal("User not found.", response.Message);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnOk_WhenUserIsDeleted()
    {
        // Arrange
        _mockUserService.Setup(s => s.DeleteUserAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteUser(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(okResult.Value);
        Assert.True(response.Data);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _mockUserService.Setup(s => s.DeleteUserAsync(1))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteUser(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(notFoundResult.Value);
        Assert.False(response.Data);
    }
}