using Microsoft.AspNetCore.Mvc;
using UserApi.Models.ResponseModels;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _service.GetAllUsersAsync();
        return Ok(new ResponseModel<IEnumerable<User>>(users));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        var createdUser = await _service.CreateUserAsync(user);
        if (createdUser == null)
        {
            return BadRequest(new ResponseModel<User>(null, "Failed to create user."));
        }
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, new ResponseModel<User>(createdUser));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _service.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new ResponseModel<User>(null, "User not found."));
        }
        return Ok(new ResponseModel<User>(user));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        var success = await _service.UpdateUserAsync(id, user);
        if (!success)
        {
            return NotFound(new ResponseModel<bool>(false, "User not found or update failed."));
        }
        return Ok(new ResponseModel<bool>(true, "User updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _service.DeleteUserAsync(id);
        if (!success)
        {
            return NotFound(new ResponseModel<bool>(false, "User not found or delete failed."));
        }
        return Ok(new ResponseModel<bool>(true, "User deleted successfully."));
    }
}