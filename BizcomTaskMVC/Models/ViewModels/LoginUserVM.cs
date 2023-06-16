using System.ComponentModel.DataAnnotations;

namespace BizcomTaskMVC.Models.ViewModels;

public class LoginUserVM
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    public string? Email { get; set; }

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}