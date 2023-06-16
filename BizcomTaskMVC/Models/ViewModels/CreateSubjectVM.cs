using System.ComponentModel.DataAnnotations;

namespace BizcomTaskMVC.Models.ViewModels;

public class CreateSubjectVM
{
    [Required]
    [MinLength(4)]
    public string Name { get; set; } = string.Empty;
}