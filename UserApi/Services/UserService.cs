using Mapster;
using Microsoft.EntityFrameworkCore;
using UserApi.Helper;
using UserApi.Models.RequestModel;
using UserApi.Models.ResponseModels;
using UserApi.Services;

public class UserService : IUserService
{
    private readonly UserDbContext _context;

    public UserService(UserDbContext context)
    {
        _context = context;
    }

    public IEnumerable<UserResponseModel> GetAllUsers()
    {
        return _context.Users.ToListAsync().Result.Adapt<List<UserResponseModel>>();
    }

    public UserResponseModel GetUserById(int id)
    {
        return _context.Users.Find(id).Adapt<UserResponseModel>();
    }

    public async Task<UserResponseModel> CreateUserAsync(UserCreateRequestModel userRM)
    {
        var hasher = new Hasher(userRM.Password);
        var user = userRM.Adapt<User>();
        user.Password = Convert.ToBase64String(hasher.ToArray());
        user.CreatedAt = DateTime.UtcNow;
        var role = await _context.Roles.FindAsync(userRM.RoleId);
        user.RoleId = role.Id;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var userDetail = userRM.Adapt<UserDetails>();
         userDetail.UserId = user.Id;
        userDetail.CreatedAt=DateTime.UtcNow;
        _context.UserDetails.Add(userDetail);
        await _context.SaveChangesAsync();
        var userResponse = user.Adapt<UserResponseModel>();
        return userResponse;
    }
    public async Task<int> ValidateUser(UserLoginRequestModel? loginRequest)
    {
        if (loginRequest == null) return 0;
        var user =  _context.Users.FirstOrDefault(x => x.Email == loginRequest.Mail);
        if(user == null) return 0;
        var hasher = new Hasher(Convert.FromBase64String(user.Password));
        if (user?.Password != null && !hasher.Verify(loginRequest.Password)) return 0;
        return user.Id;
    }

    public async Task<bool> UpdateUserAsync(int id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null) return false;

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}

