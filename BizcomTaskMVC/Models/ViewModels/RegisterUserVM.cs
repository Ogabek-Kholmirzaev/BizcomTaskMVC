using System.ComponentModel.DataAnnotations;

namespace BizcomTaskMVC.Models.ViewModels;

public class RegisterUserVM
{
    [Required]
    [RegularExpression("^[A-Za-z]+$")]
    public string? FirstName { get; set; }

    [Required]
    [RegularExpression("^[A-Za-z]+$")]
    public string? LastName { get; set; }

    [Required]
    [RegularExpression(@"\+9989\d{8}", ErrorMessage = "+9989******** ko'rinishida kiriting")]
    public string? PhoneNumber { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    public string? Email { get; set; }

    [Required]
    public string? Role { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; } = DateTime.UtcNow.Date;

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public string UserName = Guid.NewGuid().ToString();
}