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
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly string _jwtSecret;

    public UserController(IUserService service ,IConfiguration configuration)
    {
        _service = service;
        _jwtSecret = configuration["JwtSettings:Key"];

    }
    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
    public IActionResult Login([FromBody] UserLoginRequestModel userLoginRequest)
    {
        var userId =  _service.ValidateUser(userLoginRequest);
        if (userId.Result == 0) return Ok(new ResponseModel<string>("", "UserName or Password is incorrect"));

        var token = GenerateToken(userId.Result);

        return Ok(new ResponseModel<string>(token));
    }
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _service.GetAllUsers();
        return Ok(new ResponseModel<IEnumerable<UserResponseModel>>(users));
    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestModel user)
    {
        var createdUser = await _service.CreateUserAsync(user);
        if (createdUser == null)
        {
            return BadRequest(new ResponseModel<User>(null, "Failed to create user."));
        }
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser }, new ResponseModel<UserResponseModel>(createdUser));
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = _service.GetUserById(id);
        if (user == null)
        {
            return NotFound(new ResponseModel<User>(null, "User not found."));
        }
        return Ok(new ResponseModel<UserResponseModel>(user));
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
    private string GenerateToken(int id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var key = Encoding.ASCII.GetBytes(_jwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = "Test", // Issuer burada belirtiliyor
            Audience = "Test", // Audience burada belirtiliyor
            SigningCredentials =
          new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSecret)), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}