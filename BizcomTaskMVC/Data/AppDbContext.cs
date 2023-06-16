using BizcomTaskMVC.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BizcomTaskMVC.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.Migrate();
    }
    
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<AppUserSubject> AppUserSubjects { get; set; }
}