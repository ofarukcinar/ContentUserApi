
namespace UserApi.Models.RequestModel;

public class UserCreateRequestModel
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public string  Address { get; set; }
}