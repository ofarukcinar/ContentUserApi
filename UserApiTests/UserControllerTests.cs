
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly UserDbContext _context;

    public UserControllerTests()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserDb")
            .Options;
        _context = new UserDbContext(options);
        _controller = new UserController(_context);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsEmptyList()
    {
        var result = await _controller.GetAllUsers();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsType<List<User>>(okResult.Value);
        Assert.Empty(users);
    }

    [Fact]
    public async Task CreateUser_AddsUserSuccessfully()
    {
        var newUser = new User { Name = "Test", Email = "test@example.com" };
        var result = await _controller.CreateUser(newUser);
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Single(await _context.Users.ToListAsync());
    }
}
