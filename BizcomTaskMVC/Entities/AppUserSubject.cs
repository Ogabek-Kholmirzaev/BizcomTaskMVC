using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizcomTaskMVC.Entities;

public class AppUserSubject
{
    public Guid Id { get; set; }

    [Required]
    [Range(0, 100)]
    public int Grade { get; set; }
    public bool IsTeacher { get; set; }
    public bool IsAdmin { get; set; }

    public Guid AppUserId { get; set; }
    [ForeignKey(nameof(AppUserId))] 
    public virtual AppUser? AppUser { get; set; }

    public Guid SubjectId { get; set; }
    [ForeignKey(nameof(SubjectId))] 
    public virtual Subject? Subject { get; set; }
}