using System.ComponentModel.DataAnnotations;

namespace BizcomTaskMVC.Models.ViewModels;

public class JoinSubjectVM
{
    [Required]
    public string? Key { get; set; }
}