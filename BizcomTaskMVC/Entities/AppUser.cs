using Microsoft.AspNetCore.Identity;

namespace BizcomTaskMVC.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int StudentRegNumber { get; set; }

    public virtual List<AppUserSubject>? Subjects { get; set; }
}