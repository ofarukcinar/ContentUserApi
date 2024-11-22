using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserApi.Models.RequestModel;
using UserApi.Models.ResponseModels;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Authorize]
[Route("users")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly string _jwtSecret;
    private readonly IUserService _service;

    public UserController(IUserService service, IConfiguration configuration)
    {
        _service = service;
        _jwtSecret = configuration["JwtSettings:Key"];
    }

    /// <summary>
    /// Authenticates a user and generates a JWT token.
    /// </summary>
    /// <param name="userLoginRequest">The login credentials of the user.</param>
    /// <returns>A JWT token if the credentials are valid.</returns>
    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<string>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<string>))]
    public IActionResult Login([FromBody] UserLoginRequestModel userLoginRequest)
    {
        var userId = _service.ValidateUser(userLoginRequest);
        if (userId.Result == 0)
            return Ok(new ResponseModel<string>("", "UserName or Password is incorrect"));

        var token = GenerateToken(userId.Result);
        return Ok(new ResponseModel<string>(token));
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of all users.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<IEnumerable<UserResponseModel>>))]
    public IActionResult GetAllUsers()
    {
        var users = _service.GetAllUsers();
        return Ok(new ResponseModel<IEnumerable<UserResponseModel>>(users));
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">The details of the user to be created.</param>
    /// <returns>The created user.</returns>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseModel<UserResponseModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<string>))]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestModel user)
    {
        var createdUser = await _service.CreateUserAsync(user);
        if (createdUser == null)
            return BadRequest(new ResponseModel<User>(null, "Failed to create user."));

        return CreatedAtAction(nameof(GetUserById), new { id = createdUser },
            new ResponseModel<UserResponseModel>(createdUser));
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The requested user.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<UserResponseModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
    public IActionResult GetUserById(int id)
    {
        var user = _service.GetUserById(id);
        if (user == null)
            return NotFound(new ResponseModel<User>(null, "User not found."));
        return Ok(new ResponseModel<UserResponseModel>(user));
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="user">The updated user data.</param>
    /// <returns>Success status of the update operation.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        var success = await _service.UpdateUserAsync(id, user);
        if (!success)
            return NotFound(new ResponseModel<bool>(false, "User not found or update failed."));
        return Ok(new ResponseModel<bool>(true, "User updated successfully."));
    }

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>Success status of the delete operation.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _service.DeleteUserAsync(id);
        if (!success)
            return NotFound(new ResponseModel<bool>(false, "User not found or delete failed."));
        return Ok(new ResponseModel<bool>(true, "User deleted successfully."));
    }

    /// <summary>
    /// Generates a JWT token for a given user ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>A JWT token.</returns>
    private string GenerateToken(int id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = "Test",
            Audience = "Test",
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
