using System.ComponentModel.DataAnnotations;

namespace ContentApi.Models.RequestModel;

public class CreateContentRequestModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title must be a maximum of 100 characters.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Body is required.")]
    [StringLength(1000, ErrorMessage = "Body must be a maximum of 1000 characters.")]
    public string Body { get; set; }
}