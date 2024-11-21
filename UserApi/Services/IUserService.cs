using UserApi.Models.RequestModel;
using UserApi.Models.ResponseModels;

namespace UserApi.Services;

public interface IUserService
{
    IEnumerable<UserResponseModel> GetAllUsers();
    UserResponseModel GetUserById(int id);
    Task<UserResponseModel> CreateUserAsync(UserCreateRequestModel user);
    Task<bool> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
    Task<int> ValidateUser(UserLoginRequestModel? loginRequest);
}