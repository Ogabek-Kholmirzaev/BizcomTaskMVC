using System.ComponentModel.DataAnnotations;

namespace BizcomTaskMVC.Entities;

public class Subject
{
    public Guid Id { get; set; }

    [Required]
    [MinLength(4)]
    public string? Name { get; set; }
    public string? Key { get; set; }

    public virtual List<AppUserSubject>? AppUsers { get; set; }
}