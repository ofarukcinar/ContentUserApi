using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.RequestModel;

public class UserCreateRequestModel
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name must be a maximum of 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "RoleId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "RoleId must be a positive number.")]
    public int RoleId { get; set; }

    [StringLength(200, ErrorMessage = "Address must be a maximum of 200 characters.")]
    public string Address { get; set; }
}